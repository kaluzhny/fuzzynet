/*
 * 
 * fuzzynet: Fuzzy Logic Library for Microsoft .NET
 * Copyright (C) 2008 Dmitry Kaluzhny  (kaluzhny_dmitrie@mail.ru)
 * 
 * */

using System;
using System.Collections.Generic;


namespace AI.Fuzzy.Library
{
    /// <summary>
    /// Mamdani fuzzy inference system
    /// </summary>
    public class MamdaniFuzzySystem : GenericFuzzySystem
    {
        List<FuzzyVariable> _output = new List<FuzzyVariable>();
        List<MamdaniFuzzyRule> _rules = new List<MamdaniFuzzyRule>();

        ImplicationMethod _implMethod = ImplicationMethod.Min;
        AggregationMethod _aggrMethod = AggregationMethod.Max;
        DefuzzificationMethod _defuzzMethod = DefuzzificationMethod.Centroid;

        /// <summary>
        /// Default constructor
        /// </summary>
        public MamdaniFuzzySystem()
        {
        }

        /// <summary>
        /// Output linguistic variables
        /// </summary>
        public List<FuzzyVariable> Output
        {
            get { return _output; }
        }

        /// <summary>
        /// Fuzzy rules
        /// </summary>
        public List<MamdaniFuzzyRule> Rules
        {
            get { return _rules; }
        }

        /// <summary>
        /// Implication method
        /// </summary>
        public ImplicationMethod ImplicationMethod
        {
            get { return _implMethod; }
            set { _implMethod = value; }
        }

        /// <summary>
        /// Aggregation method
        /// </summary>
        public AggregationMethod AggregationMethod
        {
            get { return _aggrMethod; }
            set { _aggrMethod = value; }
        }

        /// <summary>
        /// Defuzzification method
        /// </summary>
        public DefuzzificationMethod DefuzzificationMethod
        {
            get { return _defuzzMethod; }
            set { _defuzzMethod = value; }
        }

        /// <summary>
        /// Get output linguistic variable by its name
        /// </summary>
        /// <param name="name">Variable's name</param>
        /// <returns>Found variable</returns>
        public FuzzyVariable OutputByName(string name)
        {
            foreach (FuzzyVariable var in _output)
            {
                if (var.Name == name)
                {
                    return var;
                }
            }

            throw new KeyNotFoundException();
        }

        /// <summary>
        /// Create new empty rule
        /// </summary>
        /// <returns></returns>
        public MamdaniFuzzyRule EmptyRule()
        {
            return new MamdaniFuzzyRule();
        }

        /// <summary>
        /// Parse rule from the string
        /// </summary>
        /// <param name="rule">String containing the rule</param>
        /// <returns></returns>
        public MamdaniFuzzyRule ParseRule(string rule)
        {
            return RuleParser<MamdaniFuzzyRule, FuzzyVariable, FuzzyTerm>.Parse(rule, EmptyRule(), Input, Output);
        }

        /// <summary>
        /// Calculate output values
        /// </summary>
        /// <param name="inputValues">Input values (format: variable - value)</param>
        /// <returns>Output values (format: variable - value)</returns>
        public Dictionary<FuzzyVariable, double> Calculate(Dictionary<FuzzyVariable, double> inputValues)
        {
            //
            // There should be one rule as minimum
            //
            if (_rules.Count == 0)
            {
                throw new Exception("There should be one rule as minimum.");
            }

            //
            // Fuzzification step
            //
            Dictionary<FuzzyVariable, Dictionary<FuzzyTerm, double>> fuzzifiedInput =
                Fuzzify(inputValues);

            //
            // Evaluate the conditions
            //
            Dictionary<MamdaniFuzzyRule, double> evaluatedConditions = EvaluateConditions(fuzzifiedInput);

            //
            // Do implication for each rule
            //
            Dictionary<MamdaniFuzzyRule, IMembershipFunction> implicatedConclusions = Implicate(evaluatedConditions);

            //
            // Aggrerate the results
            //
            Dictionary<FuzzyVariable, IMembershipFunction> fuzzyResult = Aggregate(implicatedConclusions);

            //
            // Defuzzify the result
            //
            Dictionary<FuzzyVariable, double> result = Defuzzify(fuzzyResult);

            return result;
        }


        #region Intermidiate calculations

        /// <summary>
        /// Evaluate conditions 
        /// </summary>
        /// <param name="fuzzifiedInput">Input in fuzzified form</param>
        /// <returns>Result of evaluation</returns>
        public Dictionary<MamdaniFuzzyRule, double> EvaluateConditions(Dictionary<FuzzyVariable, Dictionary<FuzzyTerm, double>> fuzzifiedInput)
        {
            Dictionary<MamdaniFuzzyRule, double> result = new Dictionary<MamdaniFuzzyRule,double>();
            foreach (MamdaniFuzzyRule rule in Rules)
            {
                result.Add(rule, EvaluateCondition(rule.Condition, fuzzifiedInput));
            }

            return result;
        }


        /// <summary>
        /// Implicate rule results
        /// </summary>
        /// <param name="conditions">Rule conditions</param>
        /// <returns>Implicated conclusion</returns>
        public Dictionary<MamdaniFuzzyRule, IMembershipFunction> Implicate(Dictionary<MamdaniFuzzyRule, double> conditions)
        {
            Dictionary<MamdaniFuzzyRule, IMembershipFunction> conclusions = new Dictionary<MamdaniFuzzyRule, IMembershipFunction>();
            foreach (MamdaniFuzzyRule rule in conditions.Keys)
            {
                MfCompositionType compType;
                switch (_implMethod)
                {
                    case ImplicationMethod.Min:
                        compType = MfCompositionType.Min;
                        break;
                    case ImplicationMethod.Production:
                        compType = MfCompositionType.Prod;
                        break;
                    default:
                        throw new Exception("Internal error.");
                }

                CompositeMembershipFunction resultMf = new CompositeMembershipFunction(
                    compType,
                    new ConstantMembershipFunction(conditions[rule]),
                    ((FuzzyTerm)rule.Conclusion.Term).MembershipFunction);
                conclusions.Add(rule, resultMf);
            }

            return conclusions;
        }

        
        /// <summary>
        /// Aggregate results
        /// </summary>
        /// <param name="conclusions">Rules' results</param>
        /// <returns>Aggregated fuzzy result</returns>
        public Dictionary<FuzzyVariable, IMembershipFunction> Aggregate(Dictionary<MamdaniFuzzyRule, IMembershipFunction> conclusions)
        {
            Dictionary<FuzzyVariable, IMembershipFunction> fuzzyResult = new Dictionary<FuzzyVariable, IMembershipFunction>();
            foreach (FuzzyVariable var in Output)
            {
                List<IMembershipFunction> mfList = new List<IMembershipFunction>();
                foreach (MamdaniFuzzyRule rule in conclusions.Keys)
                {
                    if (rule.Conclusion.Var == var)
                    {
                        mfList.Add(conclusions[rule]);
                    }
                }

                MfCompositionType composType;
                switch (_aggrMethod)
                {
                    case AggregationMethod.Max:
                        composType = MfCompositionType.Max;
                        break;
                    case AggregationMethod.Sum:
                        composType = MfCompositionType.Sum;
                        break;
                    default:
                        throw new Exception("Internal exception.");
                }
                fuzzyResult.Add(var, new CompositeMembershipFunction(composType, mfList));
            }

            return fuzzyResult;
        }

        /// <summary>
        /// Calculate crisp result for each rule
        /// </summary>
        /// <param name="fuzzyResult"></param>
        /// <returns></returns>
        public Dictionary<FuzzyVariable, double> Defuzzify(Dictionary<FuzzyVariable, IMembershipFunction> fuzzyResult)
        {
            Dictionary<FuzzyVariable, double> crispResult = new Dictionary<FuzzyVariable, double>();
            foreach (FuzzyVariable var in fuzzyResult.Keys)
            {
                crispResult.Add(var, Defuzzify(fuzzyResult[var], var.Min, var.Max));
            }

            return crispResult;
        }

        #endregion


        #region Helpers

        double Defuzzify(IMembershipFunction mf, double min, double max)
        {
            if (_defuzzMethod == DefuzzificationMethod.Centroid)
            {
                int k = 50;
                double step = (max - min) / k;

                //
                // Calculate a center of gravity as integral
                //
                double ptLeft = 0.0;
                double ptCenter = 0.0;
                double ptRight = 0.0;

                double valLeft = 0.0;
                double valCenter = 0.0;
                double valRight = 0.0;

                double val2Left = 0.0;
                double val2Center = 0.0;
                double val2Right = 0.0;

                double numerator = 0.0;
                double denominator = 0.0;
                for (int i = 0; i < k; i++)
                {
                    if (i == 0)
                    {
                        ptRight = min;
                        valRight = mf.GetValue(ptRight);
                        val2Right = ptRight * valRight;
                    }

                    ptLeft = ptRight;
                    ptCenter = min + step * ((double)i + 0.5);
                    ptRight = min + step * (i + 1);

                    valLeft = valRight;
                    valCenter = mf.GetValue(ptCenter);
                    valRight = mf.GetValue(ptRight);

                    val2Left = val2Right;
                    val2Center = ptCenter * valCenter;
                    val2Right = ptRight * valRight;

                    numerator += step * (val2Left + 4 * val2Center + val2Right) / 3.0;
                    denominator += step * (valLeft + 4 * valCenter + valRight) / 3.0;
                }

                return numerator / denominator;
            }
            else if (_defuzzMethod == DefuzzificationMethod.Bisector)
            {
                // TODO:
                throw new NotSupportedException();
            }
            else if (_defuzzMethod == DefuzzificationMethod.AverageMaximum)
            {
                // TODO:
                throw new NotSupportedException();
            }
            else
            {
                throw new Exception("Internal exception.");
            }
        }

        #endregion
    }
}
