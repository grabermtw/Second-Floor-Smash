# Customizing Characters

### Adding Unique Animations
1. Choose the character that you want to add custom animations for, and then navigate to their corresponding folder within Assets/Characters.
2. Select the asset called "<your character's name> Anim". This is the character's animation override controller.
3. In the inspector, you will see a list of animations and their overrides. If no unique animations have yet been given to this character, then they will not have any overrides.
4. Get whatever unique animations you want to use for this character (get them from Mixamo or make them yourself, doesn't matter), and put them within a folder called "Animations" within your character's folder.
5. Select your character's animation override controller and then drag in your animations to override whichever animations in the base character controller that you want to override.

### Adding Custom Sound Effects
1. Choose the character that you want to add custom sound effects for, and then navigate to their corresponding folder within Assets/Characters.
2. Put your character's sound effects in a folder within your character's folder called "Audio".
3. Select all of the prefabs of your character that you want to add the sound effects for.
4. In the inspector, scroll down to your character's "CharacterAudioManager" component. You will see a list of moves such as "UpPunch," "NeutralSpecial," "TakeDamage," etc. These all correspond to moves that the character performs.
5. Each of the listed moves (except for the three taunts at the bottom) are actually arrays that are empty by default. Click the arrow next to a move you want to add a sound effect for, expand its length to the number of sound effects you want to add for that move (it will pick randomly which gets played), and then drag the sound effects into the AudioClip slot(s) that appear(s).
6. Taunt audio works the same way, but just take one audio clip per taunt.
