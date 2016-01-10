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
    /// Linguistic term
    /// </summary>
    public class FuzzyTerm : NamedValueImpl
    {
        IMembershipFunction _mf;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">Term name</param>
        /// <param name="mf">Membership function initially associated with the term</param>
        public FuzzyTerm(string name, IMembershipFunction mf) : base(name)
        {
            _mf = mf;
        }

        /// <summary>
        /// Membership function initially associated with the term
        /// </summary>
        public IMembershipFunction MembershipFunction
        {
            get { return _mf; }
        }
    }
}
