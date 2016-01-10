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
    /// Sugeno fuzzy inference system
    /// </summary>
    public class SugenoFuzzySystem : GenericFuzzySystem
    {
        List<SugenoVariable> _output = new List<SugenoVariable>();
        List<SugenoFuzzyRule> _rules = new List<SugenoFuzzyRule>();            

        /// <summary>
        /// Default constructor
        /// </summary>
        public SugenoFuzzySystem()
        {}

        /// <summary>
        /// Output of the system
        /// </summary>
        public List<SugenoVariable> Output
        {
            get { return _output; }
        }

        /// <summary>
        /// List of rules of the system
        /// </summary>
        public List<SugenoFuzzyRule> Rules
        {
            get { return _rules; }
        }

        /// <summary>
        /// Get the output variable of the system by name
        /// </summary>
        /// <param name="name">Name of the variable</param>
        /// <returns>Found variable</returns>
        public SugenoVariable OutputByName(string name)
        {
            foreach (SugenoVariable var in _output)
            {
                if (var.Name == name)
                {
                    return var;
                }
            }

            throw new KeyNotFoundException();
        }

        /// <summary>
        /// Use this method to create a linear function for the Sugeno fuzzy system
        /// </summary>
        /// <param name="name">Name of the function</param>
        /// <param name="coeffs">List of coefficients. List length must be less or equal to the input lenght.</param>
        /// <param name="constValue"></param>
        /// <returns>Created function</returns>
        public LinearSugenoFunction CreateSugenoFunction(string name, Dictionary<FuzzyVariable, double> coeffs, double constValue)
        {
            return new LinearSugenoFunction(name, this.Input, coeffs, constValue);
        }

        /// <summary>
        /// Use this method to create a linear function for the Sugeno fuzzy system
        /// </summary>
        /// <param name="name">Name of the function</param>
        /// <param name="coeffs">List of coefficients. List length must be less or equal to the input lenght.</param>
        /// <returns>Created function</returns>
        public LinearSugenoFunction CreateSugenoFunction(string name, double[] coeffs)
        {
            return new LinearSugenoFunction(name, this.Input, coeffs);
        }

        /// <summary>
        /// Use this method to create an empty rule for the system
        /// </summary>
        /// <returns>Created rule</returns>
        public SugenoFuzzyRule EmptyRule()
        {
            return new SugenoFuzzyRule();
        }

        /// <summary>
        /// Use this method to create rule by its textual representation
        /// </summary>
        /// <param name="rule">Rule in text form</param>
        /// <returns>Created rule</returns>
        public SugenoFuzzyRule ParseRule(string rule)
        {
            return RuleParser<SugenoFuzzyRule, SugenoVariable, ISugenoFunction>.Parse(rule, EmptyRule(), Input, Output);
        }

        /// <summary>
        /// Evaluate conditions
        /// </summary>
        /// <param name="fuzzifiedInput">Input in fuzzified form</param>
        /// <returns>Result of evaluation</returns>
        public Dictionary<SugenoFuzzyRule, double> EvaluateConditions(Dictionary<FuzzyVariable, Dictionary<FuzzyTerm, double>> fuzzifiedInput)
        {
            Dictionary<SugenoFuzzyRule, double> result = new Dictionary<SugenoFuzzyRule, double>();
            foreach (SugenoFuzzyRule rule in Rules)
            {
                result.Add(rule, EvaluateCondition(rule.Condition, fuzzifiedInput));
            }

            return result;
        }

        /// <summary>
        /// Calculate functions' results
        /// </summary>
        /// <param name="inputValues">Input values</param>
        /// <returns>Results</returns>
        public Dictionary<SugenoVariable, Dictionary<ISugenoFunction, double>> EvaluateFunctions(Dictionary<FuzzyVariable, double> inputValues)
        {
            Dictionary<SugenoVariable, Dictionary<ISugenoFunction, double>> result = new Dictionary<SugenoVariable, Dictionary<ISugenoFunction, double>>();

            foreach (SugenoVariable var in Output)
            {
                Dictionary<ISugenoFunction, double> varResult = new Dictionary<ISugenoFunction, double>();

                foreach (ISugenoFunction func in var.Functions)
                {
                    varResult.Add(func, func.Evaluate(inputValues));
                }

                result.Add(var, varResult);
            }

            return result;
        }

        /// <summary>
        /// Combine results of functions and rule evaluation
        /// </summary>
        /// <param name="ruleWeights">Rule weights (results of evaluation)</param>
        /// <param name="functionResults">Result of functions evaluation</param>
        /// <returns>Result of calculations</returns>
        public Dictionary<SugenoVariable, double> CombineResult(Dictionary<SugenoFuzzyRule, double> ruleWeights, Dictionary<SugenoVariable, Dictionary<ISugenoFunction, double>> functionResults)
        {
            Dictionary<SugenoVariable, double> numerators = new Dictionary<SugenoVariable, double>();
            Dictionary<SugenoVariable, double> denominators = new Dictionary<SugenoVariable, double>();
            Dictionary<SugenoVariable, double> results = new Dictionary<SugenoVariable, double>();

            //
            // Calculate numerator and denominator separately for each output
            //
            foreach (SugenoVariable var in Output)
            {
                numerators.Add(var, 0.0);
                denominators.Add(var, 0.0);
            }

            foreach (SugenoFuzzyRule rule in ruleWeights.Keys)
            {
                SugenoVariable var = rule.Conclusion.Var;
                double z = functionResults[var][rule.Conclusion.Term];
                double w = ruleWeights[rule];

                numerators[var] += z * w;
                denominators[var] += w;
            }

            //
            // Calculate the fractions
            //
            foreach (SugenoVariable var in Output)
            {
                if (denominators[var] == 0.0)
                {
                    results[var] = 0.0;
                }
                else
                {
                    results[var] = numerators[var] / denominators[var];
                }
            }

            return results;
        }

        /// <summary>
        /// Calculate output of fuzzy system
        /// </summary>
        /// <param name="inputValues">Input values</param>
        /// <returns>Output values</returns>
        public Dictionary<SugenoVariable, double> Calculate(Dictionary<FuzzyVariable, double> inputValues)
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
            Dictionary<SugenoFuzzyRule, double> ruleWeights = EvaluateConditions(fuzzifiedInput);

            //
            // Functions evaluation
            //
            Dictionary<SugenoVariable, Dictionary<ISugenoFunction, double>> functionsResult = EvaluateFunctions(inputValues);

            //
            // Combine output
            //
            Dictionary<SugenoVariable, double> result = CombineResult(ruleWeights, functionsResult);

            return result;
        }
    }
}
