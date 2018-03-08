using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XamPass.Models
{
    /// <summary>
    /// Enum to specify a FieldOfStudies object by setting the Type
    /// </summary>
    public enum FieldOfStudiesType
    {
        /// <summary>
        /// Fields of Studies of the Type natural science
        /// </summary>
        NaturalScience = 0,

        /// <summary>
        /// Fields of Studies of the Type social science
        /// </summary>
        SocialScience = 1,

        /// <summary>
        /// Fields of Studies of the Type economics
        /// </summary>
        Economics = 2,

        /// <summary>
        /// Fields of Studies of the Type arts
        /// </summary>
        Arts = 3,

        /// <summary>
        /// Fields of Studies of the Type engineering
        /// </summary>
        Engineering = 4
    }
}
