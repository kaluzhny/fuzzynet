/*
 * 
 * fuzzynet: Fuzzy Logic Library for Microsoft .NET
 * Copyright (C) 2008 Dmitry Kaluzhny  (kaluzhny_dmitrie@mail.ru)
 * 
 * */

using System;
using System.Collections.Generic;
using System.Text;


namespace AI.Fuzzy.Library
{
    #region Helper classes

    /// <summary>
    /// This interface must be implemented by values in parsable rules
    /// </summary>
    public interface INamedValue
    {
        /// <summary>
        /// Name of the value
        /// </summary>
        string Name { get; set; }
    }

    /// <summary>
    /// This interface must be implemented by variable in parsable rules
    /// </summary>
    public interface INamedVariable
    {
        /// <summary>
        /// Name of the variable
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// List of values that belongs to the variable
        /// </summary>
        System.Collections.Generic.List<INamedValue> Values { get; }
    }

    /// <summary>
    /// Named variable
    /// </summary>
    public abstract class NamedVariableImpl : AI.Fuzzy.Library.INamedVariable
    {
        string _name = string.Empty;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">Name of the variable</param>
        public NamedVariableImpl(string name)
        {
            if (NameHelper.IsValidName(name))
            {
                throw new ArgumentException("Invalid variable name.");
            }

            _name = name;
        }

        /// <summary>
        /// Variable name
        /// </summary>
        public string Name
        {
            get { return _name; }
            set
            {
                if (NameHelper.IsValidName(value))
                {
                    throw new ArgumentException("Invalid variable name.");
                }

                _name = value;
            }
        }

        /// <summary>
        /// Named values
        /// </summary>
        public abstract List<INamedValue> Values { get; }
    }

    /// <summary>
    /// Named value of variable
    /// </summary>
    public abstract class NamedValueImpl : INamedValue
    {
        string _name;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">Name of the value</param>
        public NamedValueImpl(string name)
        {
            if (NameHelper.IsValidName(name))
            {
                throw new ArgumentException("Invalid term name.");
            }

            _name = name;
        }

        /// <summary>
        /// Name of the term
        /// </summary>
        public string Name
        {
            set
            {
                if (NameHelper.IsValidName(value))
                {
                    throw new ArgumentException("Invalid term name.");
                }

                _name = value;
            }
            get
            {
                return _name;
            }
        }
    }

    internal class NameHelper
    {
        static public string[] KEYWORDS = new string[] { "if", "then", "is", "and", "or", "not", "(", ")", "slightly", "somewhat", "very", "extremely" };

        /// <summary>
        /// Check the name of variable/term.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        static public bool IsValidName(string name)
        {
            //
            // Empty names are not allowed
            //
            if (name.Length == 0)
            {
                return false;
            }

            for (int i = 0; i < name.Length; i++)
            {
                //
                // Only letters, numbers or '_' are allowed
                //
                if (!System.Char.IsDigit(name, i) ||
                    !System.Char.IsDigit(name, i) ||
                    name[i] != '_')
                {
                    return false;
                }
            }

            //
            // Identifier cannot be a keword
            //
            foreach (string keyword in KEYWORDS)
            {
                if (name == keyword)
                {
                    return false;
                }
            }

            return true;
        }
    }

    #endregion


    /// <summary>
    /// Class responsible for parsing
    /// </summary>
    internal class RuleParser<RuleType, OutputVariableType, OutputValueType>
        where OutputVariableType : class, INamedVariable
        where OutputValueType : class, INamedValue
        where RuleType : class, IParsableRule<OutputVariableType, OutputValueType>
    {
        #region Rule expression hierarchy
        interface IExpression
        {
            string Text { get; }
        }

        abstract class Lexem : IExpression
        {
            public abstract string Text { get; }
            public override string ToString()
            {
                return this.Text;
            }
        }

        class ConditionExpression : IExpression
        {
            List<IExpression> _expressions = null;
            FuzzyCondition _condition = null;

            public ConditionExpression(List<IExpression> expressions, FuzzyCondition condition)
            {
                _expressions = expressions;
                _condition = condition;
            }

            public List<IExpression> Expressions
            {
                get { return _expressions; }
                set { _expressions = value; }
            }

            public FuzzyCondition Condition
            {
                get { return _condition; }
                set { _condition = value; }
            }

            public string Text
            {
                get
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (IExpression ex in _expressions)
                    {
                        sb.Append(ex.Text);
                    }
                    return sb.ToString();
                }
            }
        }

        class KeywordLexem : Lexem
        {
            string _name;

            public KeywordLexem(string name)
            {
                _name = name;
            }

            public override string Text
            {
                get { return _name; }
            }
        }

        class VarLexem<VariableType> : Lexem
            where VariableType : class, INamedVariable
        {
            VariableType _var = null;
            bool _input = true;

            public VarLexem(VariableType var, bool input)
            {
                _var = var;
                _input = input;
            }

            public VariableType Var
            {
                get { return _var; }
                set { _var = value; }
            }

            public override string Text
            {
                get { return _var.Name; }
            }

            public bool Input
            {
                get { return _input; }
                set { _input = value; }
            }
        }

        interface IAltLexem
        {
            IAltLexem Alternative { get; set;}
        }

        class TermLexem<ValueType> : Lexem, IAltLexem
            where ValueType : class, INamedValue
        {
            ValueType _term = null;
            IAltLexem _alternative = null;
            bool _input = true;

            public TermLexem(ValueType term, bool input)
            {
                _term = term;
                _input = input;
            }

            public ValueType Term
            {
                get { return _term; }
                set { _term = value; }
            }

            public override string Text
            {
                get { return _term.Name; }
            }

            public IAltLexem Alternative
            {
                get { return _alternative; }
                set { _alternative = value; }
            }
        }
        #endregion

        static private Dictionary<string, Lexem> BuildLexemsList(List<FuzzyVariable> input, List<OutputVariableType> output)
        {
            Dictionary<string, Lexem> lexems = new Dictionary<string, Lexem>();

            foreach (string keyword in NameHelper.KEYWORDS)
            {
                KeywordLexem keywordLexem = new KeywordLexem(keyword);
                lexems.Add(keywordLexem.Text, keywordLexem);
            }

            foreach (FuzzyVariable var in input)
            {
                BuildLexemsList<FuzzyVariable, FuzzyTerm>(var, true, lexems);
            }

            foreach (OutputVariableType var in output)
            {
                BuildLexemsList<OutputVariableType, OutputValueType>(var, false, lexems);
            }

            return lexems;
        }

        static private void BuildLexemsList<VariableType, ValueType>(VariableType var, bool input, Dictionary<string, Lexem> lexems)
            where VariableType : class, INamedVariable
            where ValueType : class, INamedValue
        {
            VarLexem<VariableType> varLexem = new VarLexem<VariableType>(var, input);
            lexems.Add(varLexem.Text, varLexem);
            foreach (ValueType term in var.Values)
            {
                TermLexem<ValueType> termLexem = new TermLexem<ValueType>(term, input);

                Lexem foundLexem = null;
                if (!lexems.TryGetValue(termLexem.Text, out foundLexem))
                {
                    //
                    // There are no lexems with the same text. Just insert new lexem.
                    //
                    lexems.Add(termLexem.Text, termLexem);
                }
                else
                {
                    if (foundLexem is IAltLexem)
                    {
                        //
                        // There can be more than one terms with the same name.
                        // TODO: But only if they belong to defferent variables.
                        //
                        IAltLexem foundTermLexem = (IAltLexem)foundLexem;
                        while (foundTermLexem.Alternative != null)
                        {
                            foundTermLexem = foundTermLexem.Alternative;
                        }
                        foundTermLexem.Alternative = termLexem;
                    }
                    else
                    {
                        //
                        // Only terms of different vatiables can have the same name
                        //
                        throw new System.Exception(string.Format("Found more than one lexems with the same name: {0}", termLexem.Text));
                    }
                }
            }
        }

        static private List<IExpression> ParseLexems(string rule, Dictionary<string, Lexem> lexems)
        {
            List<IExpression> expressions = new List<IExpression>();

            string []words = rule.Split(' ');
            foreach (string word in words)
            {
                Lexem lexem;
                if (lexems.TryGetValue(word, out lexem))
                {
                    expressions.Add(lexem);
                }
                else
                {
                    throw new System.Exception(string.Format("Unknown identifier: {0}", word));
                }
            }

            return expressions;
        }

        static private List<IExpression> ExtractSingleCondidtions(List<IExpression> conditionExpression, List<FuzzyVariable> input, Dictionary<string, Lexem> lexems)
        {
            List<IExpression> copyExpressions = conditionExpression.GetRange(0, conditionExpression.Count);
            List<IExpression> expressions = new List<IExpression>();

            while (copyExpressions.Count > 0)
            {
                if (copyExpressions[0] is VarLexem<FuzzyVariable>)
                {
                    //
                    // Parse variable
                    //
                    VarLexem<FuzzyVariable> varLexem = (VarLexem<FuzzyVariable>)copyExpressions[0];
                    if (copyExpressions.Count < 3)
                    {
                        throw new Exception(string.Format("Condition strated with '{0}' is incorrect.", varLexem.Text));
                    }

                    if (varLexem.Input == false)
                    {
                        throw new Exception("The variable in condition part must be an input variable.");
                    }

                    //
                    // Parse 'is' lexem
                    //
                    Lexem exprIs = (Lexem)copyExpressions[1];
                    if (exprIs != lexems["is"])
                    {
                        throw new Exception(string.Format("'is' keyword must go after {0} identifier.", varLexem.Text));
                    }


                    //
                    // Parse 'not' lexem (if exists)
                    //
                    int cur = 2;
                    bool not = false;
                    if (copyExpressions[cur] == lexems["not"])
                    {
                        not = true;
                        cur++;

                        if (copyExpressions.Count <= cur)
                        {
                            throw new Exception("Error at 'not' in condition part of the rule.");
                        }
                    }

                    //"slightly"
                    //"somewhat"
                    //"very"
                    //"extremely"

                    //
                    // Parse hedge modifier (if exists)
                    //
                    HedgeType hedge = HedgeType.None;
                    if (copyExpressions[cur] == lexems["slightly"])
                    {
                        hedge = HedgeType.Slightly;
                    }
                    else if (copyExpressions[cur] == lexems["somewhat"])
                    {
                        hedge = HedgeType.Somewhat;
                    }
                    else if (copyExpressions[cur] == lexems["very"])
                    {
                        hedge = HedgeType.Very;
                    }
                    else if (copyExpressions[cur] == lexems["extremely"])
                    {
                        hedge = HedgeType.Extremely;
                    }

                    if (hedge != HedgeType.None)
                    {
                        cur++;

                        if (copyExpressions.Count <= cur)
                        {
                            throw new Exception(string.Format("Error at '{0}' in condition part of the rule.", hedge.ToString().ToLower()));
                        }
                    }

                    //
                    // Parse term
                    //
                    Lexem exprTerm = (Lexem)copyExpressions[cur];
                    if (!(exprTerm is IAltLexem))
                    {
                        throw new Exception(string.Format("Wrong identifier '{0}' in conditional part of the rule.", exprTerm.Text));
                    }

                    IAltLexem altLexem = (IAltLexem)exprTerm;
                    TermLexem<FuzzyTerm> termLexem = null;
                    do
                    {
                        if (!(altLexem is TermLexem<FuzzyTerm>))
                        {
                            continue;
                        }

                        termLexem = (TermLexem<FuzzyTerm>)altLexem;
                        if (!varLexem.Var.Values.Contains(termLexem.Term))
                        {
                            termLexem = null;
                            continue;
                        }
                    }
                    while ((altLexem = altLexem.Alternative) != null && termLexem == null);

                    if (termLexem == null)
                    {
                        throw new Exception(string.Format("Wrong identifier '{0}' in conditional part of the rule.", exprTerm.Text));
                    }

                    //
                    // Add new condition expression
                    //
                    FuzzyCondition condition = new FuzzyCondition(varLexem.Var, termLexem.Term, not, hedge);
                    expressions.Add(new ConditionExpression(copyExpressions.GetRange(0, cur + 1), condition));
                    copyExpressions.RemoveRange(0, cur + 1);
                }
                else
                {
                    IExpression expr = copyExpressions[0];
                    if (expr == lexems["and"] ||
                        expr == lexems["or"] ||
                        expr == lexems["("] ||
                        expr == lexems[")"])
                    {
                        expressions.Add(expr);
                        copyExpressions.RemoveAt(0);
                    }
                    else
                    {
                        Lexem unknownLexem = (Lexem)expr;
                        throw new Exception(string.Format("Lexem '{0}' found at the wrong place in condition part of the rule.", unknownLexem.Text));
                    }
                }
            }

            return expressions;
        }

        static private Conditions ParseConditions(List<IExpression> conditionExpression, List<FuzzyVariable> input, Dictionary<string, Lexem> lexems)
        {
            //
            // Extract single conditions
            //
            List<IExpression> expressions = ExtractSingleCondidtions(conditionExpression, input, lexems);

            if (expressions.Count == 0)
            {
                throw new Exception("No valid conditions found in conditions part of the rule.");
            }

            ICondition cond = ParseConditionsRecurse(expressions, lexems);

            //
            // Return conditions
            //
            if (cond is Conditions)
            {
                return (Conditions)cond;
            }
            else
            {
                Conditions conditions = new Conditions();
                return conditions;
            }
        }

        static private int FindPairBracket(List<IExpression> expressions, Dictionary<string, Lexem> lexems)
        {
            //
            // Assume that '(' stands at first place
            //

            int bracketsOpened = 1;
            int closeBracket = -1;
            for (int i = 1; i < expressions.Count; i++)
            {
                if (expressions[i] == lexems["("])
                {
                    bracketsOpened++;
                }
                else if (expressions[i] == lexems[")"])
                {
                    bracketsOpened--;
                    if (bracketsOpened == 0)
                    {
                        closeBracket = i;
                        break;
                    }
                }
            }

            return closeBracket;
        }

        static private ICondition ParseConditionsRecurse(List<IExpression> expressions, Dictionary<string, Lexem> lexems)
        {
            if (expressions.Count < 1)
            {
                throw new Exception("Empty condition found.");
            }

            if (expressions[0] == lexems["("] && FindPairBracket(expressions, lexems) == expressions.Count)
            {
                //
                // Remove extra brackets
                //
                return ParseConditionsRecurse(expressions.GetRange(1, expressions.Count - 2), lexems);
            }
            else if (expressions.Count == 1 && expressions[0] is ConditionExpression)
            {
                //
                // Return single conditions
                //
                return ((ConditionExpression)expressions[0]).Condition;
            }
            else
            {
                //
                // Parse list of one level conditions connected by or/and
                //
                List<IExpression> copyExpressions = expressions.GetRange(0, expressions.Count);
                Conditions conds = new Conditions();
                bool setOrAnd = false;
                while (copyExpressions.Count > 0)
                {
                    ICondition cond = null;
                    if (copyExpressions[0] == lexems["("])
                    {
                        //
                        // Find pair bracket
                        //
                        int closeBracket = FindPairBracket(copyExpressions, lexems);
                        if (closeBracket == -1)
                        {
                            throw new Exception("Parenthesis error.");
                        }

                        cond = ParseConditionsRecurse(copyExpressions.GetRange(1, closeBracket - 1), lexems);
                        copyExpressions.RemoveRange(0, closeBracket + 1);
                    }
                    else if (copyExpressions[0] is ConditionExpression)
                    {
                        cond = ((ConditionExpression)copyExpressions[0]).Condition;
                        copyExpressions.RemoveAt(0);
                    }
                    else
                    {
                        throw new ArgumentException(string.Format("Wrong expression in condition part at '{0}'"),copyExpressions[0].Text );
                    }

                    //
                    // And condition to the list
                    //
                    conds.ConditionsList.Add(cond);

                    if (copyExpressions.Count > 0)
                    {
                        if (copyExpressions[0] == lexems["and"] || copyExpressions[0] == lexems["or"])
                        {
                            if (copyExpressions.Count < 2)
                            {
                                throw new Exception(string.Format("Error at {0} in condition part.", copyExpressions[0].Text));
                            }

                            //
                            // Set and/or for conditions list
                            //
                            OperatorType newOp = (copyExpressions[0] == lexems["and"]) ? OperatorType.And : OperatorType.Or;

                            if (setOrAnd)
                            {
                                if (conds.Op != newOp)
                                {
                                    throw new Exception("At the one nesting level cannot be mixed and/or operations.");
                                }
                            }
                            else
                            {
                                conds.Op = newOp;
                                setOrAnd = true;
                            }
                            copyExpressions.RemoveAt(0);
                        }
                        else
                        {
                            throw new Exception(string.Format("{1} cannot goes after {0}", copyExpressions[0].Text, copyExpressions[1].Text));
                        }
                    }
                }

                return conds;
            }
        }

        static private SingleCondition<VariableType, ValueType> ParseConclusion<VariableType, ValueType>(List<IExpression> conditionExpression, List<VariableType> output, Dictionary<string, Lexem> lexems)
            where VariableType : class, INamedVariable
            where ValueType : class, INamedValue
        {

            List<IExpression> copyExpression = conditionExpression.GetRange(0, conditionExpression.Count);

            //
            // Remove extra brackets
            //
            while (
                copyExpression.Count >= 2 &&
                (copyExpression[0] == lexems["("] && copyExpression[conditionExpression.Count - 1] == lexems[")"]))
            {
                copyExpression = copyExpression.GetRange(1, copyExpression.Count - 2);
            }

            if (copyExpression.Count != 3)
            {
                throw new Exception("Conclusion part of the rule should be in form: 'variable is term'");
            }

            //
            // Parse variable
            //
            Lexem exprVariable = (Lexem)copyExpression[0];
            if (!(exprVariable is VarLexem<VariableType>))
            {
                throw new Exception(string.Format("Wrong identifier '{0}' in conclusion part of the rule.", exprVariable.Text));
            }

            VarLexem<VariableType> varLexem = (VarLexem<VariableType>)exprVariable;
            if (varLexem.Input == true)
            {
                throw new Exception("The variable in conclusion part must be an output variable.");
            }
            
            //
            // Parse 'is' lexem
            //
            Lexem exprIs = (Lexem)copyExpression[1];
            if (exprIs != lexems["is"])
            {
                throw new Exception(string.Format("'is' keyword must go after {0} identifier.", varLexem.Text));
            }

            //
            // Parse term
            //
            Lexem exprTerm = (Lexem)copyExpression[2];
            if (!(exprTerm is IAltLexem))
            {
                throw new Exception(string.Format("Wrong identifier '{0}' in conclusion part of the rule.", exprTerm.Text));
            }

            IAltLexem altLexem = (IAltLexem)exprTerm;
            TermLexem<ValueType> termLexem = null;
            do
            {
                if (!(altLexem is TermLexem<ValueType>))
                {
                    continue;
                }

                termLexem = (TermLexem<ValueType>)altLexem;
                if (!varLexem.Var.Values.Contains(termLexem.Term))
                {
                    termLexem = null;
                    continue;
                }
            }
            while ((altLexem = altLexem.Alternative) != null && termLexem == null);

            if (termLexem == null)
            {
                throw new Exception(string.Format("Wrong identifier '{0}' in conclusion part of the rule.", exprTerm.Text));
            }

            //
            // Return fuzzy rule's conclusion
            //
            return new SingleCondition<VariableType, ValueType>(varLexem.Var, termLexem.Term, false);
        }

        static internal RuleType Parse(string rule, RuleType emptyRule, List<FuzzyVariable> input, List<OutputVariableType> output)
        {
            if (rule.Length == 0)
            {
                throw new ArgumentException("Rule cannot be empty.");
            }

            //
            // Surround brakes with spaces, remove double spaces
            //
            System.Text.StringBuilder sb = new StringBuilder();
            foreach (char ch in rule)
            {
                if (ch == ')' || ch == '(')
                {
                    if (sb.Length > 0 && sb[sb.Length - 1] == ' ')
                    {
                        // Do not duplicate spaces
                    }
                    else
                    {
                        sb.Append(' ');
                    }

                    sb.Append(ch);
                    sb.Append(' ');
                }
                else
                {
                    if (ch == ' ' && sb.Length > 0 && sb[sb.Length - 1] == ' ')
                    {
                        // Do not duplicate spaces
                    }
                    else
                    {
                        sb.Append(ch);
                    }
                }
            }

            //
            // Remove spaces
            //
            string prepRule = sb.ToString().Trim();

            //
            // Build lexems dictionary
            //
            Dictionary<string, Lexem> lexemsDict = BuildLexemsList(input, output);

            //
            // At first we parse lexems
            //
            List<IExpression> expressions = ParseLexems(prepRule, lexemsDict);
            if (expressions.Count == 0)
            {
                throw new System.Exception("No valid identifiers found.");
            }

            //
            // Find condition & conclusion parts part
            //
            if (expressions[0] != lexemsDict["if"])
            {
                throw new System.Exception("'if' should be the first identifier.");
            }

            int thenIndex = -1;
            for (int i = 1; i < expressions.Count; i++)
            {
                if (expressions[i] == lexemsDict["then"])
                {
                    thenIndex = i;
                    break;
                }
            }

            if (thenIndex == -1)
            {
                throw new System.Exception("'then' identifier not found.");
            }

            int conditionLen = thenIndex - 1;
            if (conditionLen < 1)
            {
                throw new System.Exception("Condition part of the rule not found.");
            }

            int conclusionLen = expressions.Count - thenIndex - 1;
            if (conclusionLen < 1)
            {
                throw new System.Exception("Conclusion part of the rule not found.");
            }

            List<IExpression> conditionExpressions = expressions.GetRange(1, conditionLen);
            List<IExpression> conclusionExpressions = expressions.GetRange(thenIndex + 1, conclusionLen);

            Conditions conditions = ParseConditions(conditionExpressions, input, lexemsDict);
            SingleCondition<OutputVariableType, OutputValueType> conclusion = ParseConclusion<OutputVariableType, OutputValueType>(conclusionExpressions, output, lexemsDict);

            emptyRule.Condition = conditions;
            emptyRule.Conclusion = conclusion;
            return emptyRule;
        }
    }
}
