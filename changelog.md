### 2019-12-19
[fartwhif]
* Fix infinite loop when viewing a packet marked with a parsing exception under extra info column

### 2019-06-20
[Mag-nus]
* The logic for OpcodeFinder and TextSearchers has been separated from the UI
* IpHeader added to PacketRecord
* Added ServerList which includes retail server information
* ACE dll's added to the /lib folder
* aclogview/ACE Helpers/ created
* Created a framework to run multiple pcap scrapers simultaneously
* Pcap Scraper includes a "Exclude Non-Retail Pcaps" option
* Menu item "Heatmap" has been removed and can now be run by menu item "Pcap Scraper"
* Menu item "Packet Type Counts" has been removed and can now be run by menu item "Pcap Scraper"
* ServerAddressScraper added
* PacketSizeScraperC2S added
* PlayerExporter added
* Account/CharacterList exporter added

### 2019-05-18
[Slushnas]
* Fixed display issue with AllegianceInfoResponseEvent.
* Added some missing messages to CM_Game.
* Added context info to CM_Fellowship, CM_Game, CM_House, CM_Trade, and CM_Physics.

### 2019-05-08
[Slushnas]
* Fixed display grouping of Monarch/Patron/Self/Vassals in AllegianceUpdate messages.
* Added context info to CM_Allegiance messages.

### 2019-02-07
[OptimShi]
* Added Evt_DDD__Interrogation_ID, Evt_DDD__InterrogationResponse_ID, Evt_DDD__BeginDDD_ID, Evt_DDD__Data_ID, Evt_DDD__EndDDD_ID and Evt_DDD__RequestData_ID event handling. These are all related to patching dat files and version control.

### 2018-09-28
[Slushnas]
* Added context info for CM_Communication, CM_Social, CM_Character, CM_Death, CM_Misc, CM_Examine, and CM_Train.
* Fixed an issue with the SpellbookFilter enum.

### 2018-09-14
[Slushnas]
* Added context info for CM_Movement, CM_Item, and CM_Combat.
* Some fields that were not serialized will now be displayed in gray in the TreeView (UpdatePosition quaternions etc.).
* Added custom ListView class to support context menus on column headers.
* Prevented context menu opening on Created Objects listview and main listview column headers.
* Added ability to hide columns on the main listview by right-clicking on the column headers.
* Fixed some time format display issues.
* Fixed an exception when clicking "Expand All" in the treeview context menu.

### 2018-09-14
[Morosity/Spazmodica]
* Updated .NET to 4.7.1
* Added Server port referenced for both outgoing and incoming packets

### 2018-08-30
[fartwhif]
* Added seq, queue, and iteration columns to main list.
* Added **"ACE style packet header flag nomenclature"** option to the main listview.
* Added lookup enum for MessageQueue column.

### 2018-08-28
[Slushnas]
* Added microsecond time precision display to the main listview.
* Fixed an issue with the time field not being displayed properly for .pcapng files.
* Added **"Copy time field"** option to the main listview.

### 2018-08-25
[Slushnas]
* Added an option to pad the treeview *Copy All* output so it doesn't have a flat structure. The default is 3 spaces.
* Added support for creating/copying ACE **@teleloc** lines from CreateObject, UpdatePosition, and MoveToState messages.

### 2018-03-19
[Slushnas]
* Added context info to CM_Qualities and added/improved parsing of property values.
* Fixed parsing issues with a couple messages.
* Renamed listview Time column to indicate the epoch time format and added a feature to display local time in the options menu.
* Added/fixed various supporting enums.
* Refactored and broke out some parsing into classes to avoid code duplication.
* Fixed an exception when clicking on the Options menu Cancel button.

### 2018-03-03
[Slushnas]
* Added context info to CM_Advocate and CM_Vendor.
* Added treeview tooltip to display the nodes context info data type. This option is off by default but can be toggled on in the options menu.
* Fixed a small bug when checking for protocol documentation updates.
* Fixed exception in fragments mode when clicking an a row with no opcode.
* Fixed bug where protocol documentation wasn't being updated when clicking on the same opcode with a different packet direction (C2S vs. S2C).

### 2018-02-25
[Slushnas]
* Added a feature to view an offline version of protocol documentation on the main form. The latest documentation was taken from Zegeger's website and is now hosted at <a href="https://acemulator.github.io">https://acemulator.github.io</a>. 
The user will be prompted to download the documentation on the first run and will check for updates by default every five days.
Selecting a message and clicking on the _Protocol Documentation_ tab will view the information.
* Added an options menu to Tools -> Options. For now it only includes the ability to turn on/off protocol update checks and adjust the update interval.

### 2018-02-04
[Slushnas]
* Added context info to CM_Writing.
* Added context info to Proto_UI.
* Refactored and fixed some message display code.
* Refactoring and other small changes were made to the ContextInfo class and its implementation to better align with my current understanding of recommended practices.
* Fixed a bug that prevented PLAYER_DESCRIPTION messages from being displayed properly in some cases.
* Fixed some small issues with treeview expansion and collapse features to better align with intuitive behavior.
* Corrected context info selection code: entries with a length of zero are used to clear the selection in some cases.

### 2018-01-29
[Dworkin]
* Put timestamps in MovementData in the correct order.

[Slushnas]
* Added context info to CM_Inventory and made a couple minor adjustments to CM_Character.

### 2018-01-26
[Slushnas]
##### Interface Changes
* Made some changes to the hexbox control to improve scrolling functionality when using context info highlighting.
* Changed row click behavior of **_Find Opcode In Files_** and **_Find Text In Files_** so that double-clicking anywhere in the row will open the file.
* Added fix to main form to prevent hex data from being re-drawn in messages mode every time the user selects a tree view node.
* Added a **_Go To Line_** feature under the Edit menu to allow the user to input a line number and jump to it.
* Added some temporary code to prevent context info from being used in messages that are not supported yet.

##### Other Changes
* Added context info to CM_Admin.
* Added context info to messages in CM_Login and other supporting classes.
* Did some small refactoring of CM_Magic context info and added info to the PurgeEnchantments and PurgeBadEnchantments messages for completeness.
* Added context info supporting code to common packed object types.
* Changed the readToAlign function to return the number of padding bytes read.

### 2018-01-19
[Slushnas]
##### Interface Changes
* Added a tab control and added the hex data view to it. This was done so that new tabs with additional functionality can be added easily.
* Added Be.Windows.Forms.HexBox control to the solution and changed the hex view to use it instead of the RichTextBox. This control supports shadow highlighting as well as other features and provides an easy to use interface to programmatically select data.
* Added context menu for copying hex or text from the hexbox control.
* Added keyboard shortcuts to hex view copy functions.
* Changed File->Open items to "(Re)Open As Fragments" to clarify the mode and moved the menu item below "Open As Messages" since fragment mode is not widely used.
* Added keyboard shortcuts to "(Re)Open As Messages" menu items.
* Refactored highlighting code in "Fragments" mode to use the new hex control.
* Changed "Copy" item to "Copy All" in the treeview to indicate that it copies the entire tree to the clipboard.
* Changed some of the text displayed to the user when using search(message highlighting) features to be more intuitive when no results are found.
* Fixed display of hex opcode when opening a new file.
* Changed position of items on the Tools menu. The "Find" options are used more often and are now at the top.

##### Other Changes
* Did a small refactor of CM_Magic code to use the EnchantmentID class.
* Added ContextInfo class and code in the main form to support it. This allows highlighting hex view data when selecting a tree view node and opens up the possibility of future context aware features.
* Added context info to CM_Magic messages.

### 2018-01-02
[Slushnas]
* Added a simple highlight UINT32 feature for currently opened files. This can be used to search for Object IDs, DIDs, etc.
* Added a _Highlight Object ID_ function to the created objects list context menu.
* Fixed a case where an exception could be thrown.
* Small refactor of the _Find ID In Object List_ option.
* Prevent context menus from opening when there are no items present.
* When jumping to a message from the objects listview context menu the currently viewed packet number is now updated properly.

### 2017-12-16
[Slushnas]
* Migrated to .NET framework 4.6.1 to keep in line with the ACEmulator/ACE project.
* Added Command Line Parser NuGet package and refactored command line argument parsing to use it.
* Fixed display of the weenie description location field.
* Changed some of the folder structure of the project.
##### Interface Changes and New Features
* Added text search functionality to open pcaps and groups of pcaps via the **_Find Text In Files_** feature. This feature supports searching for case-sensitive and case-insensitive ASCII and Unicode (UTF-16) strings.
* Added a **_Created Objects_** listview that will display information from Physics__CreateObject__ID messages in a pcap to hopefully make identification of object IDs easier. 
    You can right-click on an item in this new listview and jump to that message in the main packet listview. 
    There is also a new context menu option in the parsed data treeview that allows you to search for a selected object ID in the objects listview.
* Added double buffering to main form list views to prevent flickering.
* Made a few small changes to the interface to make it more intuitive and consistent. (Showing wait cursors, showing number of highlighted opcode messages, fixing a case where the top row would not be selected when using "Next Highlighted Row" etc.)

### 2017-12-09
[Slushnas]
* Fixed an issue with **_Find Opcode In Files_** where pcaps were not being fully processed in "as messages" mode. This should only have affected parsing for the special output tab.
* Converted special output to use the StringBuilder class and AppendText which results in much better performance.
* Changed the special output RichTextBox to read only so that the user can't accidentally alter the output.
* Fixed some typos.

### 2017-11-27
[Slushnas]
* Renamed MotionStyle enum to MotionCommand and modified it to support the values from the latest client version.
* Modified the command_ids array in CM_Movement.cs to fix parsing of movement actions.

### 2017-11-25
[Slushnas]
* Changed **_Find Opcode In Files_** to process files in "as messages" mode which fixes some cases where messages being searched for would not be found.

### 2017-11-20
[Slushnas]
* Added multiple hex and enum conversions to multiple messages.
* Fixed support for UpdateString and UpdateFloat messages.
* Fixed some issues with the Trade class and AcceptTrade message.
* Added support for the CharacterError message and added some info to the enum for it.
* Fixed up LogOff and CharacterDelete messages.
* Changed data types in ACCharGenResult and added some enum conversions.
* Added support for the AccountBooted message.
* Added support for Fellowship AssignNewLeader message and added some fixes to the FellowshipFullUpdate message.
* Fixed alignment issue with PlayerTeleport and DeleteObject messages.
* Fixed some issues with the CharacterCreate message and added some supporting enums.
* Renamed LifestoneMaterialize message to PositionAndMovement.
* Added PositionPack class and converted UpdatePosition message to use the PositionPack class.
* Added MovementData and MovementDataUnpack classes and did some refactoring so that the MovementEvent and PositionAndMovement messages as well as the PhysicsDesc class parse movement data correctly.
* Renamed MovementAction class to ActionNode because it is named that way in the client and also corrected parsing of the data.
* Fixed JumpPack class parsing and by extension the Jump message.
* Added the display of packed bitfield items and hex conversions to PhysicsDesc and PublicWeenieDesc classes.
* Added the display of PhysicsState items to the SetState message.
* Fixed parsing of the ChatServerData message and added some supporting enums.
* Made the CM_Movement class public.
* Added support for the Join, Quit, Stalemate, Recv_JoinGameResponse, and Recv_GameOver chess messages and added some related enums.
* Reworked the StatMod class to use the EnchantmentTypeEnum and fixed an issue with the enum order.
* Removed the ContentProfile class from CM_Login and CM_Trade as they are duplicates of CM_Inventory.ContentProfile.
* Added ContainerProperties enum and added it to ContentProfile display.
* Fixed display of some messages that use the INVENTORY_LOC enum.
* Added some int properties to the STypeInt enum.
* Added bitfield items and hex conversion to many fields in the PlayerDescription class.
* Added Weenie class IDs enum (WCLASSID) with data from the client_portal.dat file.

### 2017-11-03
[Slushnas]
##### Interface Change
* Fixed bug when moving to next highlighted row where rows on the last page would not be selected.
* Added double buffering to treeview class by using a custom override. This should prevent most flicker in the treeview when updating.
I also had to set the build platform target to *any* instead of *x64* to avoid loading errors in the form designer. This is expected behavior that is explained here: https://support.microsoft.com/en-us/help/963017/cannot-add-controls-from-64-bit-assemblies-to-the-toolbox-or-use-in-de
* Refactored treeview node expansion state and TopNode code.
* Renamed the utility function Utility.FormatGuid to Utility.FormatHex and **_Display Guid as Hex_** to **_Display Data as Hex_** since it is used for more fields than just Guids. (Example: bitmasks)
* Added wait cursor when loading a pcap and when toggling **_Display Data as Hex_**.
* Fixed bug where the currently viewed line number was not being updated properly when moving between highlighted items.

##### Other Changes
* Fixed spellbook parsing in the AppraisalInfo message.
* Added support for the House_Recv_UpdateRestrictions, and House_Recv_UpdateHAR messages and added an enum.
* Added RadarColor, RDBBitmask, and CoverageMask enums.
* Added RestrictionDB field to PublicWeenieDescription.
* Fixed up multiple fields in PublicWeenieDescription.
* Added enum conversion to VendorProfile.
* Did some cleanup to the Login_CharacterSet message.
* Added changelog to the solution.

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
