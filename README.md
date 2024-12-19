# DOTS Space Shooter

## Splines
The enemies move along splines, provided by [Unity's Splines Package](https://docs.unity3d.com/Packages/com.unity.splines@2.4/manual/index.html).<br>
For performance and compatibility with ECS I use [Native Splines](https://docs.unity3d.com/Packages/com.unity.splines@2.1/api/UnityEngine.Splines.NativeSpline.html), which are read-only representations of splines.<br>
[Why they should be blobbed](https://discussions.unity.com/t/what-options-for-splines-in-ecs-do-you-use-1-0-14/927439/3)