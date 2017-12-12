/* Rewired Constants
   This list was generated on 12/11/2017 9:38:19 PM
   The list applies to only the Rewired Input Manager from which it was generated.
   If you use a different Rewired Input Manager, you will have to generate a new list.
   If you make changes to the exported items in the Rewired Input Manager, you will need to regenerate this list.
*/

namespace RewiredConsts {
    public static class Action {
        // Default
        // UI
        [Rewired.Dev.ActionIdFieldInfo(categoryName = "UI", friendlyName = "Nav Horizontal")]
        public const int Nav_Horizontal = 6;
        [Rewired.Dev.ActionIdFieldInfo(categoryName = "UI", friendlyName = "Nav Vertical")]
        public const int Nav_Vertical = 7;
        [Rewired.Dev.ActionIdFieldInfo(categoryName = "UI", friendlyName = "Select")]
        public const int Select = 8;
        [Rewired.Dev.ActionIdFieldInfo(categoryName = "UI", friendlyName = "Cancel")]
        public const int Cancel = 9;
        [Rewired.Dev.ActionIdFieldInfo(categoryName = "UI", friendlyName = "Confirm")]
        public const int Confirm = 10;
        // Gameplay
        [Rewired.Dev.ActionIdFieldInfo(categoryName = "Gameplay", friendlyName = "Move Horizontal")]
        public const int Move_Horizontal = 0;
        [Rewired.Dev.ActionIdFieldInfo(categoryName = "Gameplay", friendlyName = "Aim Horizontal")]
        public const int Aim_Horizontal = 1;
        [Rewired.Dev.ActionIdFieldInfo(categoryName = "Gameplay", friendlyName = "Aim Vertical")]
        public const int Aim_Vertical = 2;
        [Rewired.Dev.ActionIdFieldInfo(categoryName = "Gameplay", friendlyName = "Jump")]
        public const int Jump = 3;
        [Rewired.Dev.ActionIdFieldInfo(categoryName = "Gameplay", friendlyName = "Spin Left")]
        public const int Spin_Left = 4;
        [Rewired.Dev.ActionIdFieldInfo(categoryName = "Gameplay", friendlyName = "Spin Right")]
        public const int Spin_Right = 5;
    }
    public static class Category {
        public const int Default = 0;
    }
    public static class Layout {
        public static class Joystick {
            public const int Default = 0;
        }
        public static class Keyboard {
            public const int Default = 0;
        }
        public static class Mouse {
            public const int Default = 0;
        }
        public static class CustomController {
            public const int Default = 0;
        }
    }
}
