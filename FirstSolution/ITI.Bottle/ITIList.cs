﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Bottle
{
    public class ITIList<T>
    {
        T[] _tab;
        int _count;

        public ITIList()
        {
            _tab = new T[4];
        }

        public int Count 
        {
            get { return _count; } 
        }

        public T this[ int i ]
        {
            get 
            {
                if( i >= _count ) throw new IndexOutOfRangeException();
                return _tab[i];
            }
            set 
            {
                if( i >= _count ) throw new IndexOutOfRangeException();
                _tab[i] = value; 
            }
        } 
        
        public void Add( T b )
        {
            Debug.Assert( _count <= _tab.Length );
            if( ++_count > _tab.Length )
            {
                //var newOne = new SimpleBottle[_tab.Length * 2];
                //Array.Copy( _tab, newOne, _tab.Length );
                Array.Resize( ref _tab, _tab.Length * 2 );
            }
            _tab[_count - 1] = b;
        }

        public void Add( params T[] item )
        {
            foreach( var i in item ) Add( i );
        }

        public void RemoveAt( int i )
        {
            if( i < 0 || i >= _count ) throw new IndexOutOfRangeException();
            int lenToCopy = --_count - i;
            if( lenToCopy > 0 ) Array.Copy( _tab, i + 1, _tab, i, lenToCopy );
            _tab[_count] = default(T);
        }

        /// <summary>
        /// Returns the index (zero based) of the first occurence of the given item. -1 if not found.
        /// </summary>
        /// <param name="item">The bottle to find.</param>
        /// <returns>Positive if found, -1 otherwise.</returns>
        public int IndexOf( T item )
        {
            for( int i = 0; i < _count; ++i )
            {
                //if( _tab[i].Equals( item ) ) return i;
                if( EqualityComparer<T>.Default.Equals( _tab[i], item ) ) return i;
            }
            return -1;
        }

        public int IndexOf( T item, int firstIndex = 0, int count = -1 )
        {
            if( firstIndex < 0 || firstIndex >= _count ) throw new IndexOutOfRangeException();
            int lastExcluded;
            if( count == -1 ) lastExcluded = _count;
            else 
            {
                lastExcluded = firstIndex + count;
                if( lastExcluded > _count ) throw new IndexOutOfRangeException();
            }
            for( int i = firstIndex; i < lastExcluded; ++i )
            {
                if( EqualityComparer<T>.Default.Equals( _tab[i], item ) ) return i;
            }
            return -1;
        }

        /// <summary>
        /// Returns the index (zero based) of the last occurence of the given item. -1 if not found.
        /// </summary>
        /// <param name="item">The item to find.</param>
        /// <returns>Positive if found, -1 otherwise.</returns>
        public int LastIndexOf( T item )
        {
            for( int i = _count-1; i >= 0; --i )
            {
                if( EqualityComparer<T>.Default.Equals( _tab[i], item ) ) return i;
            }
            return -1;
        }




        //public bool Remove( SimpleBottle b )
        //{
        //}

    }
}
