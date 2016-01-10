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
    /// Types of membership functions' composition
    /// </summary>
    public enum MfCompositionType
    {
        /// <summary>
        /// Minumum of functions
        /// </summary>
        Min,
        /// <summary>
        /// Maximum of functions
        /// </summary>
        Max,
        /// <summary>
        /// Production of functions
        /// </summary>
        Prod,
        /// <summary>
        /// Sum of functions
        /// </summary>
        Sum
    }

    /// <summary>
    /// Interface of membership function
    /// </summary>
    public interface IMembershipFunction
    {
        /// <summary>
        /// Evaluate value of the membership function
        /// </summary>
        /// <param name="x">Argument (x axis value)</param>
        /// <returns></returns>
        double GetValue(double x);
    }

    /// <summary>
    /// Triangular membership function
    /// </summary>
    public class TriangularMembershipFunction : IMembershipFunction
    {
        double _x1, _x2, _x3;

        /// <summary>
        /// Default constructor
        /// </summary>
        public TriangularMembershipFunction()
        {}

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="x1">Point 1</param>
        /// <param name="x2">Point 2</param>
        /// <param name="x3">Point 3</param>
        public TriangularMembershipFunction(double x1, double x2, double x3)
        {
            if (!(x1 <= x2 && x2 <= x3))
            {
                throw new ArgumentException();
            }

            _x1 = x1;
            _x2 = x2;
            _x3 = x3;
        }

        /// <summary>
        /// Point 1
        /// </summary>
        public double X1
        {
            get { return _x1; }
            set { _x1 = value; }
        }

        /// <summary>
        /// Point 2
        /// </summary>
        public double X2
        {
            get { return _x2; }
            set { _x2 = value; }
        }

        /// <summary>
        /// Point 3
        /// </summary>
        public double X3
        {
            get { return _x3; }
            set { _x3 = value; }
        }

        /// <summary>
        /// Evaluate value of the membership function
        /// </summary>
        /// <param name="x">Argument (x axis value)</param>
        /// <returns></returns>
        public double GetValue(double x)
        {
            double result = 0;

            if (x == _x1 && x == _x2)
            {
                result = 1.0;
            }
            else if (x == _x2 && x == _x3)
            {
                result = 1.0;
            }
            else if (x <= _x1 || x >= _x3)
            {
                result = 0;
            }
            else if (x == _x2)
            {
                result = 1;
            }
            else if ((x > _x1) && (x < _x2))
            {

                result = (x / (_x2 - _x1)) - (_x1 / (_x2 - _x1));
            }
            else
            {
                result = (-x / (_x3 - _x2)) + (_x3 / (_x3 - _x2));
            }

            return result;
        }

        /// <summary>
        /// Approximately converts to normal membership function
        /// </summary>
        /// <returns></returns>
        public NormalMembershipFunction ToNormalMF()
        {
            double b = _x2;
            double sigma25 = (_x3 - _x1) / 2.0;
            double sigma = sigma25 / 2.5;
            return new NormalMembershipFunction(b, sigma);
        }
    }


    /// <summary>
    /// Trapezoid membership function
    /// </summary>
    public class TrapezoidMembershipFunction : IMembershipFunction
    {
        double _x1, _x2, _x3, _x4;


        /// <summary>
        /// Constructor
        /// </summary>
        public TrapezoidMembershipFunction()
        {}


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="x1">Point 1</param>
        /// <param name="x2">Point 2</param>
        /// <param name="x3">Point 3</param>
        /// <param name="x4">Point 4</param>
        public TrapezoidMembershipFunction(double x1, double x2, double x3, double x4)
        {
            if (!(x1 <= x2 && x2 <= x3 && x3 <= x4))
            {
                throw new ArgumentException();
            }

            _x1 = x1;
            _x2 = x2;
            _x3 = x3;
            _x4 = x4;
        }

        /// <summary>
        /// Point 1
        /// </summary>
        public double X1
        {
            get { return _x1; }
            set { _x1 = value; }
        }

        /// <summary>
        /// Point 2
        /// </summary>
        public double X2
        {
            get { return _x2; }
            set { _x2 = value; }
        }

        /// <summary>
        /// Point 3
        /// </summary>
        public double X3
        {
            get { return _x3; }
            set { _x3 = value; }
        }

        /// <summary>
        /// Point 4
        /// </summary>
        public double X4
        {
            get { return _x4; }
            set { _x4 = value; }
        }

        /// <summary>
        /// Evaluate value of the membership function
        /// </summary>
        /// <param name="x">Argument (x axis value)</param>
        /// <returns></returns>
        public double GetValue(double x)
        {
            double result = 0;

            if (x == _x1 && x == _x2)
            {
                result = 1.0;
            }
            else if (x == _x3 && x == _x4)
            {
                result = 1.0;
            }
            else if (x <= _x1 || x >= _x4)
            {
                result = 0;
            }
            else if ((x >= _x2) && (x <= _x3))
            {
                result = 1;
            }
            else if ((x > _x1) && (x < _x2))
            {
                result = (x / (_x2 - _x1)) - (_x1 / (_x2 - _x1));
            }
            else
            {
                result = (-x / (_x4 - _x3)) + (_x4 / (_x4 - _x3));
            }

            return result;
        }
    }

    /// <summary>
    /// Normal membership function
    /// </summary>
    public class NormalMembershipFunction : IMembershipFunction
    {
        double _b = 0.0, _sigma = 1.0;

        /// <summary>
        /// Constructor
        /// </summary>
        public NormalMembershipFunction()
        {}


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="b">Parameter b (center of MF)</param>
        /// <param name="sigma">Sigma</param>
        public NormalMembershipFunction(double b, double sigma)
        {
            _b = b;
            _sigma = sigma;
        }

        /// <summary>
        /// Parameter b (center of MF)
        /// </summary>
        public double B
        {
            get { return _b; }
            set { _b = value; }
        }

        /// <summary>
        /// Sigma
        /// </summary>
        public double Sigma
        {
            get { return _sigma; }
            set { _sigma = value; }
        }

        /// <summary>
        /// Evaluate value of the membership function
        /// </summary>
        /// <param name="x">Argument (x axis value)</param>
        /// <returns></returns>
        public double GetValue(double x)
        {
            return Math.Exp(-(x - _b) * (x - _b) / (2.0 * _sigma * _sigma));
        }
    }

    /// <summary>
    /// Constant membership function
    /// </summary>
    public class ConstantMembershipFunction : IMembershipFunction
    {
        double _constValue;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="constValue">Constant value</param>
        public ConstantMembershipFunction(double constValue)
        {
            if (constValue < 0.0 || constValue > 1.0)
            {
                throw new ArgumentException();
            }

            _constValue = constValue;
        }

        /// <summary>
        /// Evaluate value of the membership function
        /// </summary>
        /// <param name="x">Argument (x axis value)</param>
        /// <returns></returns>
        public double GetValue(double x)
        {
            return _constValue;
        }
    }


    /// <summary>
    /// Composition of several membership functions represened as single membership function
    /// </summary>
    internal class CompositeMembershipFunction : IMembershipFunction
    {
        List<IMembershipFunction> _mfs = new List<IMembershipFunction>();
        MfCompositionType _composType;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="composType">Membership functions composition type</param>
        public CompositeMembershipFunction(MfCompositionType composType)
        {
            _composType = composType;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="composType">Membership functions composition type</param>
        /// <param name="mf1">Membership function 1</param>
        /// <param name="mf2">Membership function 2</param>
        public CompositeMembershipFunction(
            MfCompositionType composType,
            IMembershipFunction mf1,
            IMembershipFunction mf2) : this(composType)
        {
            _mfs.Add(mf1);
            _mfs.Add(mf2);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="composType">Membership functions composition type</param>
        /// <param name="mfs">Membership functions</param>
        public CompositeMembershipFunction(
                MfCompositionType composType,
                List<IMembershipFunction> mfs)
            : this(composType)
        {
            _mfs = mfs;
        }

        /// <summary>
        /// List of membership functions
        /// </summary>
        public List<IMembershipFunction> MembershipFunctions
        {
            get { return _mfs; }
        }

        /// <summary>
        /// Membership functions composition type
        /// </summary>
        public MfCompositionType CompositionType
        {
            get { return _composType; }
            set { _composType = value; }
        }

        /// <summary>
        /// Evaluate value of the membership function
        /// </summary>
        /// <param name="x">Argument (x axis value)</param>
        /// <returns></returns>
        public double GetValue(double x)
        {
            if (_mfs.Count == 0)
            {
                return 0.0;
            }
            else if (_mfs.Count == 1)
            {
                return _mfs[0].GetValue(x);
            }
            else
            {
                double result = _mfs[0].GetValue(x);
                for (int i = 1; i < _mfs.Count; i++)
                {
                    result = Compose(result, _mfs[i].GetValue(x));
                }
                return result;
            }
        }

        double Compose(double val1, double val2)
        {
            switch (_composType)
            {
                case MfCompositionType.Max:
                    return Math.Max(val1, val2);
                case MfCompositionType.Min:
                    return Math.Min(val1, val2);
                case MfCompositionType.Prod:
                    return val1 * val2;
                case MfCompositionType.Sum:
                    return val1 + val2;
                default:
                    throw new Exception("Internal exception.");
            }
        }
    }
}
