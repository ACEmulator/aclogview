### 2017-10-29
[Slushnas]
* Added support for the Modify, Add, and Delete book response events and added hex conversions for book "flags" fields.
* Added handler for the AllegianceUpdateDone message.
* Fixed parsing of PlayerModule structure. This fixes parsing for messages like CharacterOptionsEvent and PlayerDescription.
* Fixed gender enum and added a UIElement enum to support gameplay options in the PlayerModule structure.
* Added ulong override for the Utility.FormatGuid function.
* Fixed some gleaned cooldown spell IDs based on ACE DB.

### 2017-10-22
[Slushnas]
##### Interface Changes
* Added **_Expand All_** and **_Collapse All_** options to the parsed data treeview right-click menu.
* When toggling **_Display Guid as Hex_** the interface will now restore your position in the treeview after updating instead of scrolling you to the top or bottom.

### 2017-10-20
[Slushnas]
##### Interface Change
* Expanded the treeview pane a little to reduce the chance of seeing horizontal scrollbars.

##### Other Changes

* Added support for the AppraiseDone message.
* Fixed support for SendClientContractTrackerTable (0x0314) and SendClientContractTracker (0x0315) messages.
* Added a custom enum for contract names.
* Added support for the AttackDoneEvent.
* Added some missing character title enum IDs.
* Fixed FriendsUpdate message.
* Fixed AddOrSetCharacterTitle and SetDisplayCharacterTitle messages.
* Other small formatting fixes.

### 2017-10-18
[Slushnas]
##### Interface Changes
* The default method for opening files is now the "as messages" mode. This mainly affects opening files from the "Find Opcode In Files" dialog.
* The "Use Highlighting" checkbox will be unchecked and disabled when viewing a file in "as messages" mode as it was not designed for use in this mode.
* Fixed a bug where files opened in "as messages" mode could get highlighting applied when it shouldn't be. As a result, larger messages
will now load faster in some cases.
* You can now hit the Enter key in the Opcode box to start a search in the "Find Opcode In Files" dialog.

##### Other Changes
* Renamed Contract cooldown spell enum as it looks like it is shared for all contracts.
* Added gender enum.
* Renamed lock related weenie errors as they apply to both chests and doors.
* Added enum and Guid conversions to lots of variables.
* Added initial support for the AllegianceUpdate event (0x0020) and AllegianceInfoResponseEvent (0x027C). There is a chance that character nodes will not be grouped correctly 
in the hierarchy but all data should be parsed.
* Fixed several housing messages: BuyHouse, Recv_HouseProfile, RentHouse, Recv_UpdateRentPayment, and Recv_AvailableHouses.

### 2017-10-14
[Slushnas]
* Fixed parsing of CreateTinkeringTool and SalvageOperationsResultData messages.
* Added support for InventoryServerSaysFailedEvent, ViewContentsEvent, and InventoryPutObjIn3DEvent messages.
* Added GUID conversion and enums to many message variables.
* Renamed UpdateStackSize variable from maxNumPages to ts to indicate it is a timestamp.

### 2017-10-13
[Slushnas]
* Added support for the following communication messages: SetAFKMode, SetAFKMessage, ModifyCharacterSquelch, ModifyAccountSquelch, ModifyGlobalSquelch, and SetSquelchDB.
* Added some enums for the newly supported squelch messages.

### 2017-10-09
[Slushnas]
* The treeview will now update when toggling "Display Guid as Hex."

### 2017-10-06
[Slushnas]
* Overhauled Magic message handling to support more messages.
* Renamed spell id field to be consistent throughout.
* Commented out some messages that don't appear to be used.

### 2017-10-04
[Slushnas]
* Added support for ChannelBroadcast messages.
* Added GroupChatType enums from ACE and added a few that were found in the client.
* Changed an SMainCat enum to a more descriptive name.
* Added a couple of enum display conversions for inventory and sounds.

### 2017-09-30
[Slushnas]
* Added support for Magic__UpdateEnchantment message (02C2).
* Added some gleaned spell enums to support the UpdateEnchantment message.
* Changed GetAndWieldItem message to display GUID format.

### 2017-09-24
[OptimShi]
* Corrected the reading of PList<HousePayment> and added proper structure to the TreeView

### 2017-09-22
[OptimShi]
* Implemented "Actions" in the Movement Event (F74C)

### 2017-09-16
[OptimShi]
* Added PlayerDescriptionEvent 0x0013
* Added ability to hit the "Enter" key in the Highlight Opcode text box to perform the highlight.

### 2017-09-15
* Changelog Created.