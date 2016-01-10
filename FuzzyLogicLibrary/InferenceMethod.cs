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
    /// And evaluating method
    /// </summary>
    public enum AndMethod
    {
        /// <summary>
        /// Minimum: min(a, b)
        /// </summary>
        Min,
        /// <summary>
        /// Production: a * b
        /// </summary>
        Production
    }


    /// <summary>
    /// Or evaluating method
    /// </summary>
    public enum OrMethod
    {
        /// <summary>
        /// Maximum: max(a, b)
        /// </summary>
        Max,
        /// <summary>
        /// Probabilistic OR: a + b - a * b
        /// </summary>
        Probabilistic
    }

    /// <summary>
    /// Fuzzy implication method
    /// </summary>
    public enum ImplicationMethod
    {
        /// <summary>
        /// Truncation of output fuzzy set
        /// </summary>
        Min,
        /// <summary>
        /// Scaling of output fuzzy set
        /// </summary>
        Production
    }

    /// <summary>
    /// Aggregation method for membership functions
    /// </summary>
    public enum AggregationMethod
    {
        /// <summary>
        /// Maximum of rule outpus
        /// </summary>
        Max,
        /// <summary>
        /// Sum of rule output
        /// </summary>
        Sum
    }

    /// <summary>
    /// Defuzzification method
    /// </summary>
    public enum DefuzzificationMethod
    {
        /// <summary>
        /// Center of area of fuzzy result MF
        /// </summary>
        Centroid,
        /// <summary>
        /// Not implemented
        /// </summary>
        Bisector,
        /// <summary>
        /// Not implemented
        /// </summary>
        AverageMaximum
    }
}
