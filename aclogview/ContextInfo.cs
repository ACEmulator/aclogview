using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aclogview
{
    /// <summary>
    /// This class is used to store context info about fields in messages.
    /// </summary>
    public class ContextInfo
    {
        public static readonly Dictionary<int, ContextInfo> contextList = new Dictionary<int, ContextInfo>();
        public static int DataIndex { get; set; }
        public static int NodeIndex { get; set; }
        public int StartPosition { get; set; }
        public int Length { get; set; }
        public DataType DataType { get; set; }
        private static readonly Dictionary<DataType, int> dataTypeList = new Dictionary<DataType, int>()
        {
            { DataType.ClientToServerHeader, 12 },
            { DataType.ServerToClientHeader, 16 },
            { DataType.SpellID_uint, 4 },
            { DataType.EnchantmentID, 4 },
            { DataType.SpellID_ushort, 2 },
            { DataType.SpellLayer, 2 },
            { DataType.ObjectID, 4 },
            { DataType.DataID, 4 },
            { DataType.Opcode, 4 },
            { DataType.CellID, 4 }
        };

        // Use the default constructor to update the starting byte index each time it is 
        // instantiated so that we don't have to when adding a new item to the list.
        public ContextInfo()
        {
            StartPosition = DataIndex;
        }

        /// <summary>
        /// This method adds context info to the main list and by default
        /// updates the node index and data index variables.
        /// </summary>
        /// <param name="c"></param>
        /// <param name="updateDataIndex"></param>
        public static void AddToList(ContextInfo c, bool updateDataIndex = true) {
            if (dataTypeList.ContainsKey(c.DataType))
                c.Length = dataTypeList[c.DataType];
            else if (c.DataType != DataType.Undefined && c.Length <= 0)
                throw new InvalidOperationException("ContextInfo.AddToList Error - A defined data type must have a byte length greater than zero.");
            contextList.Add(NodeIndex, c);
            if (updateDataIndex)
            {
                DataIndex += c.Length;
            }
            NodeIndex++;
        }

        public static void Reset()
        {
            contextList.Clear();
            DataIndex = 0;
            NodeIndex = 0;
        }
    }

    public enum DataType
    {
        Undefined,

        // Fixed length
        ClientToServerHeader,            // 12 bytes
        ServerToClientHeader,            // 16 bytes
        SpellID_uint,        
        EnchantmentID,                   // uint composed of two ushorts: the spell ID and the layer
        SpellID_ushort,
        SpellLayer,     
        ObjectID,
        DataID,
        Opcode,
        CellID,                          // 4 byte landblock cell ID
        

        // Variable length
        ShortSerialized_UnicodeString,   // UTF-16 string with length < 128 (+ 1 byte length header)
        LongSerialized_UnicodeString,    // UTF-16 string with length >= 128 (+ 2 byte length header)
        UnicodeString,                   // UTF-16 string with no length header
        Serialized_AsciiString,          // AKA PStringChar; Has a 2-4 byte length header
        WCID,                            // Weenie class ID
        IconID
    }
}
