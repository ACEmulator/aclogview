using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;

namespace aclogview.Tools.Parsers
{
    class SampleParserResult
    {
        public string FileName;
        public int Hits;
        public int Exceptions;

        public readonly List<string> SpecialOutput = new List<string>();
    }

    // This is just a sample class to hold example code
    class SampleParser
    {
        private readonly ConcurrentDictionary<string, int> SpecialOutputHits = new ConcurrentDictionary<string, int>();

        public void Reset()
        {
            SpecialOutputHits.Clear();
        }

        public SampleParserResult ProcessFileRecords(string fileName, List<PacketRecord> records, ref bool searchAborted)
        {
            var result = new SampleParserResult { FileName = fileName };

            foreach (PacketRecord record in records)
            {
                if (searchAborted)
                    return result;

                // ********************************************************************
                // ************************ CUSTOM SEARCH CODE ************************ 
                // ********************************************************************
                // Custom search code that can output information to Special Output
                // Below are several commented out examples on how you can search through bulk pcaps for targeted data, and output detailed information to the output tab.

                try
                {
                    if (record.data.Length <= 4)
                        continue;

                    using (var memoryStream =new MemoryStream(record.data))
                    using (var binaryReader = new BinaryReader(memoryStream))
                    {
                        var messageCode = binaryReader.ReadUInt32();

                        if (messageCode == (uint)PacketOpcode.Evt_Movement__MovementEvent_ID)
                        {
                            var parsed = CM_Movement.MovementEvent.read(binaryReader);

                            if (parsed.movement_data.movementData_Unpack.movement_type == MovementTypes.Type.MoveToObject && $"{parsed.object_id:X8}".StartsWith("50"))
                            {
                                var output = $"{parsed.object_id:X8},{parsed.movement_data.movementData_Unpack.moveToObject:X8},{parsed.movement_data.movementData_Unpack.style},{parsed.movement_data.movementData_Unpack.movement_params.distance_to_object},{fileName},{record.index}";

                                if (SpecialOutputHits.TryAdd(parsed.movement_data.movementData_Unpack.movement_params.distance_to_object.ToString() + parsed.movement_data.movementData_Unpack.style, 0))
                                    result.SpecialOutput.Add(output);
                            }
                        }

                        if (messageCode == 0x02BB) // Creature Message
                        {
                            var parsed = CM_Communication.HearSpeech.read(binaryReader);

                            //if (parsed.ChatMessageType != 0x0C)
                            //    continue;

                            var output = parsed.ChatMessageType.ToString("X4") + " " + parsed.MessageText;

                            if (SpecialOutputHits.TryAdd(output, 0))
                                result.SpecialOutput.Add(output);
                        }

                        if (messageCode == 0xF745) // Create Object
                        {
                            var parsed = CM_Physics.CreateObject.read(binaryReader);
                        }

                        if (messageCode == 0xF7B0) // Game Event
                        {
                            var character = binaryReader.ReadUInt32(); // Character
                            var sequence = binaryReader.ReadUInt32(); // Sequence
                            var _event = binaryReader.ReadUInt32(); // Event

                            if (_event == 0x0147) // Group Chat
                            {
                                var parsed = CM_Communication.ChannelBroadcast.read(binaryReader);

                                var output = parsed.GroupChatType.ToString("X4");

                                if (SpecialOutputHits.TryAdd(output, 0))
                                    result.SpecialOutput.Add(output);
                            }

                            if (_event == 0x02BD) // Tell
                            {
                                var parsed = CM_Communication.HearDirectSpeech.read(binaryReader);

                                var output = parsed.ChatMessageType.ToString("X4");

                                if (SpecialOutputHits.TryAdd(output, 0))
                                    result.SpecialOutput.Add(output);
                            }
                        }

                        if (messageCode == 0xF7B1) // Game Action
                        {
                            var sequence = binaryReader.ReadUInt32(); // Sequence
                            var opcode = binaryReader.ReadUInt32(); // Opcode

                            if (opcode == 0x0036) // Use event
                            {
                                var parsed = CM_Inventory.UseEvent.read(binaryReader);

                                var object_id = parsed.i_object;

                                for (int i = record.index; i > 0; i--)
                                {
                                    using (BinaryReader comessageDataReader = new BinaryReader(new MemoryStream(records[i].data)))
                                    {
                                        var msgCode = comessageDataReader.ReadUInt32();

                                        if (msgCode == 0xF745) // Create Object
                                        {
                                            var parsedco = CM_Physics.CreateObject.read(comessageDataReader);

                                            if (parsedco.wdesc._type == ITEM_TYPE.TYPE_PORTAL && parsedco.object_id == object_id)
                                            {
                                                var output = $"{parsedco.wdesc._name.m_buffer},{fileName},{record.index}";

                                                if (SpecialOutputHits.TryAdd(output, 0))
                                                    result.SpecialOutput.Add(output);

                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        if (messageCode == 0xF7DE) // TurbineChat
                        {
                            var parsed = CM_Admin.ChatServerData.read(binaryReader);

                            string output = parsed.m_blobType.ToString("X2");

                            if (SpecialOutputHits.TryAdd(output, 0))
                                result.SpecialOutput.Add(output);
                        }

                        if (messageCode == 0xF7E0) // Server Message
                        {
                            var parsed = CM_Communication.TextBoxString.read(binaryReader);

                            var output = parsed.ChatMessageType.ToString("X4") + " " + parsed.MessageText + ",";
                            //var output = parsed.ChatMessageType.ToString("X4");

                            if (SpecialOutputHits.TryAdd(output, 0))
                                result.SpecialOutput.Add(output);
                        }
                    }
                }
                catch
                {
                    // Do something with the exception maybe
                }
            }

            return result;
        }
    }
}
