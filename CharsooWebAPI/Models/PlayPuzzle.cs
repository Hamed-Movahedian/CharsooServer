//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CharsooWebAPI.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class PlayPuzzle
    {
        public int PlayerID { get; set; }
        public int PuzzleID { get; set; }
        public System.DateTime Time { get; set; }
        public int MoveCount { get; set; }
        public int HintCount1 { get; set; }
        public int HintCount2 { get; set; }
        public int HintCount3 { get; set; }
        public bool Success { get; set; }
        public int Duration { get; set; }
    }
}