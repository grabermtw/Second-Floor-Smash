# Second Floor Smash
A Super Smash Brothers clone using custom assets enabling a wider variety of characters and stages. It is a work-in-progress.

### Below are some guidelines about the project structure and organization.

The DefaultCharacter has the abilities that all the custom characters will share (i.e. movement, jumping, basic attack, taking damage, dodge etc.).

The CharacterController script is made to be a generic script attatchable to any character in the game, automatically granting them the ability to move, jump, take/give damage, conduct their basic attacks, etc. It essentially gives them all the abilities of the DefaultCharacter, because that is the only custom script that the DefaultCharacter has. However, the CharacterController script also broadcasts calls to methods that can be defined for each type of special attack (neutral, side, up, down), allowing for a character-specific script to be attatched to any custom character in which these attacks can be implemented.

The DefaultCharacter animation controller should *not* be used for each unique character. It is a good starting point, but each unique character will ultimately have their own unique animations, so a copy of the DefaultCharacter animation controller should be made to be used for each custom character. (Also, the DefaultCharacter's animation controller is currently extremely messy due to the lack of sub-states. Might be worth making a copy of it (in case things don't work out) and then updating the copy to use sub-states.)

The Shared Character Assets folder contains all animations and all associated scripts that the DefaultCharacter uses, as these are used by default with all custom characters. If it is desired for a custom character to differ from the DefaultCharacter in its basic attacks or other animations, or a certain character needs something different from what the animation behavior scripts associated with the basic attacks give, *it is important that a copy of what ever is to be modified is made*, so as not to accidentally inconvenience all the other characters who are fine with the default animations/behaviors.

A folder for each custom character should be created to contain all of their character-specific assets, including animations and scripts for special attacks, as well as any modified versions of the DefaultCharacter's animations or scripts.
