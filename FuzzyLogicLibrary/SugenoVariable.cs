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
    /// Interface that must be implemented by class to be used as output function in Sugeno Fuzzy System
    /// </summary>
    public interface ISugenoFunction : INamedValue
    {
        /// <summary>
        /// Calculate result of function
        /// </summary>
        /// <param name="inputValues">Input values</param>
        /// <returns>Result of the calculation</returns>
        double Evaluate(Dictionary<FuzzyVariable, double> inputValues);
    }

    /// <summary>
    /// Lenear function for Sugeno Fuzzy System (can be created via SugenoFuzzySystem::CreateSugenoFunction methods
    /// </summary>
    public class LinearSugenoFunction : NamedValueImpl, ISugenoFunction
    {
        List<FuzzyVariable> _input = null;
        Dictionary<FuzzyVariable, double> _coeffs = new Dictionary<FuzzyVariable,double>();
        double _constValue = 0.0;

        /// <summary>
        /// Get or set constant coefficient
        /// </summary>
        public double ConstValue
        {
            get { return _constValue; }
            set { _constValue = value; }
        }

        /// <summary>
        /// Get coefficient by fuzzy variable
        /// </summary>
        /// <param name="var">Fuzzy variable</param>
        /// <returns>Coefficient's value</returns>
        public double GetCoefficient(FuzzyVariable var)
        {
            if (var == null)
            {
                return _constValue;
            }
            else
            {
                return _coeffs[var];
            }
        }

        /// <summary>
        /// Set coefficient by fuzzy variable
        /// </summary>
        /// <param name="var">Fuzzy variable</param>
        /// <param name="coeff">New value of the coefficient</param>
        public void SetCoefficient(FuzzyVariable var, double coeff)
        {
            if (var == null)
            {
                _constValue = coeff;
            }
            else
            {
                _coeffs[var] = coeff;
            }
        }

        internal LinearSugenoFunction(string name, List<FuzzyVariable> input) : base(name)
        {
            _input = input;
        }

        internal LinearSugenoFunction(string name, List<FuzzyVariable> input, Dictionary<FuzzyVariable, double> coeffs, double constValue)
            : this (name, input)
        {
            //
            // Check that all coeffecients are related to the variable from input
            //
            foreach (FuzzyVariable var in coeffs.Keys)
            {
                if (!_input.Contains(var))
                {
                    throw new ArgumentException(string.Format(
                        "Input of the fuzzy system does not contain '{0}' variable.",
                        var.Name));
                }
            }

            //
            // Initialize members
            //
            _coeffs = coeffs;
            _constValue = constValue;
        }

        internal LinearSugenoFunction(string name, List<FuzzyVariable> input, double[] coeffs)
            : this(name, input)
        {
            //
            // Check input values
            //
            if (coeffs.Length != input.Count && coeffs.Length != input.Count + 1)
            {
                throw new ArgumentException("Wrong lenght of coefficients' array");
            }
            
            //
            // Fill list of coefficients
            //
            for (int i = 0; i < input.Count; i++)
            {
                _coeffs.Add(input[i], coeffs[i]);
            }

            if (coeffs.Length == input.Count + 1)
            {
                _constValue = coeffs[coeffs.Length - 1];
            }
        }

        /// <summary>
        /// Calculate result of linear function
        /// </summary>
        /// <param name="inputValues">Input values</param>
        /// <returns>Result of the calculation</returns>
        public double Evaluate(Dictionary<FuzzyVariable, double> inputValues)
        {
            //NOTE: input values should be validated here
            double result = 0.0;

            foreach (FuzzyVariable var in _coeffs.Keys)
            {
                result += _coeffs[var] * inputValues[var];
            }
            result += _constValue;

            return result;
        }
    }

    /// <summary>
    /// Used as an output variable in Sugeno fuzzy inference system.
    /// </summary>
    public class SugenoVariable : NamedVariableImpl
    {
        List<ISugenoFunction> _functions = new List<ISugenoFunction>();

        /// <summary>
        /// Cnstructor
        /// </summary>
        /// <param name="name">Name of the variable</param>
        public SugenoVariable(string name) : base(name)
        {
        }

        /// <summary>
        /// List of functions that belongs to the variable
        /// </summary>
        public List<ISugenoFunction> Functions
        {
            get
            {
                return _functions;
            }
        }

        /// <summary>
        /// List of functions that belongs to the variable (implementation of INamedVariable)
        /// </summary>
        public override List<INamedValue> Values
        {
            get
            {
                List<INamedValue> values = new List<INamedValue>();
                foreach (ISugenoFunction val in _functions)
                {
                    values.Add(val);
                }
                return values;
            }
        }

        /// <summary>
        /// Find function by its name
        /// </summary>
        /// <param name="name">Name of the function</param>
        /// <returns>Found function</returns>
        public ISugenoFunction GetFuncByName(string name)
        {
            foreach (NamedValueImpl func in Values)
            {
                if (func.Name == name)
                {
                    return (ISugenoFunction)func;
                }
            }

            throw new KeyNotFoundException();
        }
    }
}
