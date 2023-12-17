using System;

using ACE.Database.Models.Shard;

namespace aclogview.ACE_Helpers
{
    static class ACECharacterCreator
    {
        /// <summary>
        /// Do not call this twice for the same Character
        /// This should be the first message you parse when constructiong a player
        /// </summary>
        public static void Update(CM_Login.PlayerDescription message, Character character)
        {
            character.CharacterOptions1 = (int)message.PlayerModule.options_;

            if (message.PlayerModule.shortcuts_ != null)
            {
                foreach (var value in message.PlayerModule.shortcuts_.shortCuts_)
                    character.CharacterPropertiesShortcutBar.Add(new CharacterPropertiesShortcutBar { ShortcutBarIndex = (uint)value.index_, ShortcutObjectId = value.objectID_ });
            }

            for (uint i = 0; i < message.PlayerModule.favorite_spells_.Length; i++)
            {
                if (message.PlayerModule.favorite_spells_[i] != null)
                {
                    for (uint j = 0; j < message.PlayerModule.favorite_spells_[i].list.Count; j++)
                        character.CharacterPropertiesSpellBar.Add(new CharacterPropertiesSpellBar { SpellBarNumber = i, SpellBarIndex = j, SpellId = (uint)message.PlayerModule.favorite_spells_[i].list[(int)j] });
                }
            }

            foreach (var value in message.PlayerModule.desired_comps_.hashTable)
                character.CharacterPropertiesFillCompBook.Add(new CharacterPropertiesFillCompBook { SpellComponentId = (int)value.Key, QuantityToRebuy = value.Value });

            character.SpellbookFilters = message.PlayerModule.spell_filters_;

            character.CharacterOptions2 = (int)message.PlayerModule.options2;

            // This is just window placement. For now, we don't bother exporting it
            // TODO: message.PlayerModule.m_colGameplayOptions -> player.Character.GameplayOptions
        }
    }
}
