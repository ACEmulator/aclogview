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