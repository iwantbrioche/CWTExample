
namespace CWTExample.Hooks
{
    public static class Hooks
    {
        public static void PatchHooks()
        {
            On.PlayerGraphics.InitiateSprites += PlayerGraphics_InitiateSprites;
            On.PlayerGraphics.AddToContainer += PlayerGraphics_AddToContainer;
            On.PlayerGraphics.DrawSprites += PlayerGraphics_DrawSprites;
        }


        private static void PlayerGraphics_InitiateSprites(On.PlayerGraphics.orig_InitiateSprites orig, PlayerGraphics self, RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam)
        {
            orig(self, sLeaser, rCam);

            // This if statement will only run if the `data` variable returned is not null
            if (CWT.TryGetData(self, out var data))
            {
                data.newSpriteIndex = sLeaser.sprites.Length; // Store the sprite index of our new sprite

                Array.Resize(ref sLeaser.sprites, sLeaser.sprites.Length + 1); // Resize the sLeaser with its length + 1

                sLeaser.sprites[data.newSpriteIndex] = new("LizardScaleA6"); // Create the new sprite with our newSpriteIndex as it's index


                self.AddToContainer(sLeaser, rCam, null); // Call AddToContainer so the new sprite gets added to the container
            }
        }
        private static void PlayerGraphics_AddToContainer(On.PlayerGraphics.orig_AddToContainer orig, PlayerGraphics self, RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, FContainer newContatiner)
        {
            orig(self, sLeaser, rCam, newContatiner);
            if (CWT.TryGetData(self, out var data))
            {
                // This checks if the newSpriteIndex variable is 0 or less than or equal to the sLeaser.sprites.Length
                //  This is to prevent the game trying to add your new sprite to the container if it hasn't been added to the sprite leaser yet
                if (data.newSpriteIndex == 0 || sLeaser.sprites.Length <= data.newSpriteIndex) return;

                newContatiner ??= rCam.ReturnFContainer("Midground");

                newContatiner.AddChild(sLeaser.sprites[data.newSpriteIndex]); // Add the new sprite to the container
                sLeaser.sprites[data.newSpriteIndex].MoveInFrontOfOtherNode(sLeaser.sprites[3]); // Move the new sprite in front of the head sprite
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
                sLeaser.sprites[data.newSpriteIndex].SetPosition(headDrawPos + Custom.RotateAroundOrigo(new Vector2(0f, 8f), rotation) - camPos);
                sLeaser.sprites[data.newSpriteIndex].scaleX = rotation < 0f ? -1f : 1f; // Set the scaleX to flip the sprite based on where the slugcat is facing

                // Set the sprite's rotation, the reason why 180f is being added is because the LizardScaleA6 sprite is upside down
                sLeaser.sprites[data.newSpriteIndex].rotation = rotation + 180f;
            }
        }
    }
}
