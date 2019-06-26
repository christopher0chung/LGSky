# Introloop v4.0.0

Copyright (c) 2015 Sirawat Pitaksarit, Exceed7 Experiments LP 
http://www.exceed7.com/introloop

## What did it install
Assets/Introloop
Assets/Resources/Introloop/[some template prefabs]

## How to use Introloop
With internet connection you should visit : http://www.exceed7.com/introloop

Without internet connection, here are brief instructions!

1. Right click your `AudioClip` asset you want to play in Introloop way, select `Introloop > Create IntroloopAudio`
2. Set appropriate boundaries. When playhead reaches Looping Boundary, it will return to Intro Boundary.
3. Uncheck "Preload Audio Data" on your original AudioClip recommended.
4. The namespace is `using E7.Introloop`, and link up `asmdef` to `E7.Introloop` if you use `asmdef` in your project.
5. Declare something like `public IntroloopAudio myIntroloopAudio;` in your script.
6. Drag your newly created `IntroloopAudio` asset file to the inspector slot.
7. In the script call `IntroloopPlayer.Instance.Play(myIntroloopAudio)` to play. There are `Stop`, `Pause` and `Resume` also.
8. You will notice a `GameObject` appearing in `DontDestroyOnLoad` scene. This consists of 4 `AudioSource`s that are working together to stitch up the audio for you.
9. (Extra) That `GameObject` came from a "template prefab" file located in `Assets/Resources/Introloop/`. You could setup audio mixer where the audio should go to and more there.

Questions/Problems/Suggestions : 5argon@exceed7.com
Discord : http://discord.gg/KgXRaKU