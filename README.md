# DOTS Space Shooter

## Controls
The player character follows the mouse cursor.
Shoot with left click.

## Optimization 
- I've used ISystems and BurstCompiled where applicable to make logic run faster.
- Because many entities will need access to the player to query its location, I opted to treat it like a singleton.
- I've used parallel-scheduled job to handle enemy movement due to their potential to become quite many.