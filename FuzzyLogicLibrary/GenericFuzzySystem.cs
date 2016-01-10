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
    /// Common functionality of Mamdani and Sugeno fuzzy systems
    /// </summary>
    public class GenericFuzzySystem
    {
        List<FuzzyVariable> _input = new List<FuzzyVariable>();
        AndMethod _andMethod = AndMethod.Min;
        OrMethod _orMethod = OrMethod.Max;

        /// <summary>
        /// Input linguistic variables
        /// </summary>
        public List<FuzzyVariable> Input
        {
            get { return _input; }
        }

        /// <summary>
        /// And method
        /// </summary>
        public AndMethod AndMethod
        {
            get { return _andMethod; }
            set { _andMethod = value; }
        }

        /// <summary>
        /// Or method
        /// </summary>
        public OrMethod OrMethod
        {
            get { return _orMethod; }
            set { _orMethod = value; }
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        protected GenericFuzzySystem()
        {
        }

        /// <summary>
        /// Get input linguistic variable by its name
        /// </summary>
        /// <param name="name">Variable's name</param>
        /// <returns>Found variable</returns>
        public FuzzyVariable InputByName(string name)
        {
            foreach (FuzzyVariable var in Input)
            {
                if (var.Name == name)
                {
                    return var;
                }
            }

            throw new KeyNotFoundException();
        }

        #region Intermidiate calculations

        /// <summary>
        /// Fuzzify input
        /// </summary>
        /// <param name="inputValues"></param>
        /// <returns></returns>
        public Dictionary<FuzzyVariable, Dictionary<FuzzyTerm, double>> Fuzzify(Dictionary<FuzzyVariable, double> inputValues)
        {
            //
            // Validate input
            //
            string msg;
            if (!ValidateInputValues(inputValues, out msg))
            {
                throw new ArgumentException(msg);
            }

            //
            // Fill results list
            //
            Dictionary<FuzzyVariable, Dictionary<FuzzyTerm, double>> result = new Dictionary<FuzzyVariable, Dictionary<FuzzyTerm, double>>();
            foreach (FuzzyVariable var in Input)
            {
                Dictionary<FuzzyTerm, double> resultForVar = new Dictionary<FuzzyTerm, double>();
                foreach (FuzzyTerm term in var.Terms)
                {
                    resultForVar.Add(term, term.MembershipFunction.GetValue(inputValues[var]));
                }
                result.Add(var, resultForVar);
            }

            return result;
        }

        #endregion


        #region Helpers

        /// <summary>
        /// Evaluate fuzzy condition (or conditions)
        /// </summary>
        /// <param name="condition">Condition that should be evaluated</param>
        /// <param name="fuzzifiedInput">Input in fuzzified form</param>
        /// <returns>Result of evaluation</returns>
        protected double EvaluateCondition(ICondition condition, Dictionary<FuzzyVariable, Dictionary<FuzzyTerm, double>> fuzzifiedInput)
        {
            if (condition is Conditions)
            {
                double result = 0.0;
                Conditions conds = (Conditions)condition;

                if (conds.ConditionsList.Count == 0)
                {
                    throw new Exception("Inner exception.");
                }
                else if (conds.ConditionsList.Count == 1)
                {
                    result = EvaluateCondition(conds.ConditionsList[0], fuzzifiedInput);
                }
                else
                {
                    result = EvaluateCondition(conds.ConditionsList[0], fuzzifiedInput);
                    for (int i = 1; i < conds.ConditionsList.Count; i++)
                    {
                        result = EvaluateConditionPair(result, EvaluateCondition(conds.ConditionsList[i], fuzzifiedInput), conds.Op);
                    }
                }

                if (conds.Not)
                {
                    result = 1.0 - result;
                }

                return result;
            }
            else if (condition is FuzzyCondition)
            {
                FuzzyCondition cond = (FuzzyCondition)condition;
                double result = fuzzifiedInput[(FuzzyVariable)cond.Var][(FuzzyTerm)cond.Term];

                switch (cond.Hedge)
                {
                    case HedgeType.Slightly:
                        result = Math.Pow(result, 1.0 / 3.0); //Cube root
                        break;
                    case HedgeType.Somewhat:
                        result = Math.Sqrt(result);
                        break;
                    case HedgeType.Very:
                        result = result * result;
                        break;
                    case HedgeType.Extremely:
                        result = result * result * result;
                        break;
                    default:
                        break;
                }

                if (cond.Not)
                {
                    result = 1.0 - result;
                }
                return result;
            }
            else
            {
                throw new Exception("Internal exception.");
            }
        }

        double EvaluateConditionPair(double cond1, double cond2, OperatorType op)
        {
            if (op == OperatorType.And)
            {
                if (AndMethod == AndMethod.Min)
                {
                    return Math.Min(cond1, cond2);
                }
                else if (AndMethod == AndMethod.Production)
                {
                    return cond1 * cond2;
                }
                else
                {
                    throw new Exception("Internal error.");
                }
            }
            else if (op == OperatorType.Or)
            {
                if (OrMethod == OrMethod.Max)
                {
                    return Math.Max(cond1, cond2);
                }
                else if (OrMethod == OrMethod.Probabilistic)
                {
                    return cond1 + cond2 - cond1 * cond2;
                }
                else
                {
                    throw new Exception("Internal error.");
                }
            }
            else
            {
                throw new Exception("Internal error.");
            }
        }

        private bool ValidateInputValues(Dictionary<FuzzyVariable, double> inputValues, out string msg)
        {
            msg = null;
            if (inputValues.Count != Input.Count)
            {
                msg = "Input values count is incorrect.";
                return false;
            }

            foreach (FuzzyVariable var in Input)
            {
                if (inputValues.ContainsKey(var))
                {
                    double val = inputValues[var];
                    if (val < var.Min || val > var.Max)
                    {
                        msg = string.Format("Vaulue for the '{0}' variable is out of range.", var.Name);
                        return false;
                    }
                }
                else
                {
                    msg = string.Format("Vaulue for the '{0}' variable does not present.", var.Name);
                    return false;
                }
            }

            return true;
        }

        #endregion
    }
}
