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
    // Alias for a fuzzy conclusion for Mamdani systems
    using FuzzyConclusion = SingleCondition<FuzzyVariable, FuzzyTerm>;

    // Alias for a conclusion for Sugeno fuzzy systems
    using SugenoConclusion = SingleCondition<SugenoVariable, ISugenoFunction>;

    /// <summary>
    /// Condition of fuzzy rule for the both Mamdani and Sugeno systems
    /// </summary>
    public class FuzzyCondition : SingleCondition<FuzzyVariable, FuzzyTerm>
    {
        HedgeType _hedge = HedgeType.None;

        /// <summary>
        /// Hedge modifier
        /// </summary>
        public HedgeType Hedge
        {
            get { return _hedge; }
            set { _hedge = value; }
        }

        internal FuzzyCondition(FuzzyVariable var, FuzzyTerm term)
            : this(var, term, false)
        {
        }

        internal FuzzyCondition(FuzzyVariable var, FuzzyTerm term, bool not)
            : this(var, term, not, HedgeType.None)
        {
        }

        internal FuzzyCondition(FuzzyVariable var, FuzzyTerm term, bool not, HedgeType hedge)
            : base(var, term, not)
        {
            _hedge = hedge;
        }
    }


    /// <summary>
    /// And/Or operator type
    /// </summary>
    public enum OperatorType
    {
        /// <summary>
        /// And operator
        /// </summary>
        And,
        /// <summary>
        /// Or operator
        /// </summary>
        Or
    }

    /// <summary>
    /// Hedge modifiers
    /// </summary>
    public enum HedgeType
    {
        /// <summary>
        /// None
        /// </summary>
        None,
        /// <summary>
        /// Cube root
        /// </summary>
        Slightly,
        /// <summary>
        /// Square root
        /// </summary>
        Somewhat,
        /// <summary>
        /// Square
        /// </summary>
        Very,
        /// <summary>
        /// Cube
        /// </summary>
        Extremely
    }

    /// <summary>
    /// Interface of conditions used in the 'if' expression
    /// </summary>
    public interface ICondition
    {}

    /// <summary>
    /// Single condition
    /// </summary>
    public class SingleCondition<VariableType, ValueType> : ICondition //TODO: SingleCondition must be not public
        where VariableType : class, INamedVariable
        where ValueType : class, INamedValue
    {
        VariableType _var = null;
        bool _not = false;
        ValueType _term = null;


        /// <summary>
        /// Default constructor
        /// </summary>
        internal SingleCondition()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="var">A linguistic variable to which the condition is related</param>
        /// <param name="term">A term in expression 'var is term'</param>
        internal SingleCondition(VariableType var, ValueType term)
        {
            _var = var;
            _term = term;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="var">A linguistic variable to which the condition is related</param>
        /// <param name="term">A term in expression 'var is term'</param>
        /// <param name="not">Does condition contain 'not'</param>
        internal SingleCondition(VariableType var, ValueType term, bool not)
            : this(var, term)
        {
            _not = not;
        }

        /// <summary>
        /// A linguistic variable to which the condition is related
        /// </summary>
        public VariableType Var
        {
            get { return _var; }
            set { _var = value; }
        }

        /// <summary>
        /// Is MF inverted
        /// </summary>
        public bool Not
        {
            get { return _not; }
            set { _not = value; }
        }

        /// <summary>
        /// A term in expression 'var is term'
        /// </summary>
        public ValueType Term //TODO: 'Term' is bad property name here
        {
            get { return _term; }
            set { _term = value; }
        }
    }

    /// <summary>
    /// Several conditions linked by or/and operators
    /// </summary>
    public class Conditions : ICondition
    {
        bool _not = false;
        OperatorType _op = OperatorType.And;
        List<ICondition> _Conditions = new List<ICondition>();

        /// <summary>
        /// Is MF inverted
        /// </summary>
        public bool Not
        {
            get { return _not; }
            set { _not = value; }
        }

        /// <summary>
        /// Operator that links expressions (and/or)
        /// </summary>
        public OperatorType Op
        {
            get { return _op; }
            set { _op = value; }
        }

        /// <summary>
        /// A list of conditions (single or multiples)
        /// </summary>
        public List<ICondition> ConditionsList
        {
            get { return _Conditions; }
        }
    }

    /// <summary>
    /// Interface used by rule parser
    /// </summary>
    interface IParsableRule<OutputVariableType, OutputValueType>
        where OutputVariableType : class, INamedVariable
        where OutputValueType : class, INamedValue
    {
        /// <summary>
        /// Condition (IF) part of the rule
        /// </summary>
        Conditions Condition { get; set; }

        /// <summary>
        /// Conclusion (THEN) part of the rule
        /// </summary>
        SingleCondition<OutputVariableType, OutputValueType> Conclusion { get; set; }
    }

    /// <summary>
    /// Implements common functionality of fuzzy rules
    /// </summary>
    public abstract class GenericFuzzyRule
    {
        Conditions _condition = new Conditions();

        /// <summary>
        /// Condition (IF) part of the rule
        /// </summary>
        public Conditions Condition
        {
            get { return _condition; }
            set { _condition = value; }
        }

        /// <summary>
        /// Create a single condition
        /// </summary>
        /// <param name="var">A linguistic variable to which the condition is related</param>
        /// <param name="term">A term in expression 'var is term'</param>
        /// <returns>Generated condition</returns>
        public FuzzyCondition CreateCondition(FuzzyVariable var, FuzzyTerm term)
        {
            return new FuzzyCondition(var, term);
        }

        /// <summary>
        /// Create a single condition
        /// </summary>
        /// <param name="var">A linguistic variable to which the condition is related</param>
        /// <param name="term">A term in expression 'var is term'</param>
        /// <param name="not">Does condition contain 'not'</param>
        /// <returns>Generated condition</returns>
        public FuzzyCondition CreateCondition(FuzzyVariable var, FuzzyTerm term, bool not)
        {
            return new FuzzyCondition(var, term, not);
        }

        /// <summary>
        /// Create a single condition
        /// </summary>
        /// <param name="var">A linguistic variable to which the condition is related</param>
        /// <param name="term">A term in expression 'var is term'</param>
        /// <param name="not">Does condition contain 'not'</param>
        /// <param name="hedge">Hedge modifier</param>
        /// <returns>Generated condition</returns>
        public FuzzyCondition CreateCondition(FuzzyVariable var, FuzzyTerm term, bool not, HedgeType hedge)
        {
            return new FuzzyCondition(var, term, not, hedge);
        }
    }

    /// <summary>
    /// Fuzzy rule for Mamdani fuzzy system
    /// </summary>
    public class MamdaniFuzzyRule : GenericFuzzyRule, IParsableRule<FuzzyVariable, FuzzyTerm>
    {
        FuzzyConclusion _conclusion = new FuzzyConclusion();
        double _weight = 1.0;

        /// <summary>
        /// Constructor. NOTE: a rule cannot be created directly, only via MamdaniFuzzySystem::EmptyRule or MamdaniFuzzySystem::ParseRule
        /// </summary>
        internal MamdaniFuzzyRule()
        {}

        /// <summary>
        /// Conclusion (THEN) part of the rule
        /// </summary>
        public FuzzyConclusion Conclusion
        {
            get { return _conclusion; }
            set { _conclusion = value; }
        }

        /// <summary>
        /// Weight of the rule
        /// </summary>
        public double Weight
        {
            get { return _weight; }
            set { _weight = value; }
        }
    }

    /// <summary>
    /// Fuzzy rule for Sugeno fuzzy system
    /// </summary>
    public class SugenoFuzzyRule : GenericFuzzyRule, IParsableRule<SugenoVariable, ISugenoFunction>
    {
        SugenoConclusion _conclusion = new SugenoConclusion();

        /// <summary>
        /// Constructor. NOTE: a rule cannot be created directly, only via SugenoFuzzySystem::EmptyRule or SugenoFuzzySystem::ParseRule
        /// </summary>
        internal SugenoFuzzyRule()
        {}

        /// <summary>
        /// Conclusion (THEN) part of the rule
        /// </summary>
        public SugenoConclusion Conclusion
        {
            get { return _conclusion; }
            set { _conclusion = value; }
        }
    }
}
