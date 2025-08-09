
namespace CWTExample.Hooks
{
    public static class Hooks
    {
        public static void PatchHooks()
        {
            On.PlayerGraphics.InitiateSprites += PlayerGraphics_InitiateSprites;
            On.PlayerGraphics.AddToContainer += PlayerGraphics_AddToContainer;
            On.PlayerGraphics.DrawSprites += PlayerGraphics_DrawSprites;
            On.PlayerGraphics.ApplyPalette += PlayerGraphics_ApplyPalette;
        }


        private static void PlayerGraphics_InitiateSprites(On.PlayerGraphics.orig_InitiateSprites orig, PlayerGraphics self, RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam)
        {
            orig(self, sLeaser, rCam);

            // This if statement will only run if the `data` variable returned is not null
            if (CWT.TryGetData(self, out var data))
            {
                data.sLeaserLength = sLeaser.sprites.Length; // Take in the sLeaser.sprites length for later usage
                int spritesAdded = sLeaser.sprites.Length; // Creating a variable that holds the length of the sLeaser.sprites to be incremented for resizing

                data.NewSpriteIndex = spritesAdded++; // Store the sprite index of our new sprite as well as incrementing the spritesAdded variable

                Array.Resize(ref sLeaser.sprites, spritesAdded); // Resize the sLeaser with spritesAdded as it's new length

                sLeaser.sprites[data.NewSpriteIndex] = new("LizardScaleA6"); // Create the new sprite with our NewSpriteIndex as it's index


                self.AddToContainer(sLeaser, rCam, null); // Call AddToContainer so the new sprite gets added to the container
            }
        }
        private static void PlayerGraphics_AddToContainer(On.PlayerGraphics.orig_AddToContainer orig, PlayerGraphics self, RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, FContainer newContatiner)
        {
            orig(self, sLeaser, rCam, newContatiner);
            if (CWT.TryGetData(self, out var data))
            {
                // This checks if the sLeaserLength variable is 0 or less than or equal to the sLeaser.sprites.Length
                //  This is to prevent the game trying to add your new sprite to the container if it hasn't been added to the sprite leaser yet
                if (data.sLeaserLength == 0 || sLeaser.sprites.Length <= data.sLeaserLength) return;

                newContatiner ??= rCam.ReturnFContainer("Midground");

                newContatiner.AddChild(sLeaser.sprites[data.NewSpriteIndex]); // Add the new sprite to the container
                sLeaser.sprites[data.NewSpriteIndex].MoveInFrontOfOtherNode(sLeaser.sprites[3]); // Move the new sprite in front of the head sprite
            }
        }
        private static void PlayerGraphics_DrawSprites(On.PlayerGraphics.orig_DrawSprites orig, PlayerGraphics self, RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, float timeStacker, Vector2 camPos)
        {
            orig(self, sLeaser, rCam, timeStacker, camPos);
            if (CWT.TryGetData(self, out var data))
            {
                Vector2 headDrawPos = Vector2.Lerp(self.head.lastPos, self.head.pos, timeStacker); // Get the position of the head
                float rotation = sLeaser.sprites[3].rotation; // Get the rotation of the head

                // Set the position of our new sprite to be at the head, with an offset to make it stick out more
                sLeaser.sprites[data.NewSpriteIndex].SetPosition(headDrawPos + Custom.RotateAroundOrigo(new Vector2(0f, 8f), rotation) - camPos);
                sLeaser.sprites[data.NewSpriteIndex].scaleX = rotation < 0f ? -1f : 1f; // Set the scaleX to flip the sprite based on where the slugcat is facing

                // Set the sprite's rotation, the reason why 180f is being added is because the LizardScaleA6 sprite is upside down
                sLeaser.sprites[data.NewSpriteIndex].rotation = rotation + 180f;
            }
        }
        private static void PlayerGraphics_ApplyPalette(On.PlayerGraphics.orig_ApplyPalette orig, PlayerGraphics self, RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, RoomPalette palette)
        {
            orig(self, sLeaser, rCam, palette);
            if (CWT.TryGetData(self, out var data))
            {
                sLeaser.sprites[data.NewSpriteIndex].color = self.player.ShortCutColor(); // Set the new sprite's color to the slugcat's color
            }
        }
    }
}
