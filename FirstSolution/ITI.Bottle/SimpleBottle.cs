﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Bottle
{
    /// <summary>
    /// Defines a simple basic bottle.
    /// </summary>
    public class SimpleBottle
    {
        readonly int _maxCapacity;

        /// <summary>
        /// Initializes a new <see cref="SimpleBottle"/> with a default <see cref="MaxCapacity"/> of 1000 ml.
        /// </summary>
        public SimpleBottle()
            : this( 1000 )
        {
        }

        /// <summary>
        /// Initializes a new <see cref="SimpleBottle"/> with a <see cref="MaxCapacity"/>.
        /// </summary>
        /// <param name="maxCapacityMilliLiter">Maximal capacity in milliliter.</param>
        public SimpleBottle( int maxCapacityMilliLiter )
        {
            if( maxCapacityMilliLiter < 0 ) throw new ArgumentException( "Must be strictly positive.", "maxCapacityMilliLiter" );
            _maxCapacity = maxCapacityMilliLiter;
        }

        /// <summary>
        /// Gets the maximal capcity (milliliter).
        /// </summary>
        public int MaxCapacity
        {
            get { return _maxCapacity; }
        }



    }
}
