<p align="center">
    <img src="https://raw.githubusercontent.com/sschmid/Entitas-CSharp/develop/Readme/Images/Entitas-Header.png" alt="Entitas">
</p>

---

* <a href="http://www.RivelloMultimediaConsulting.com/unity/">Rivello Multimedia Consulting</a> (RMC) created this simple template, designed as a starting point for new Entitas projects.
* Entitas is an ECS (Entity Component System) which presents a new way to think about architecting your Unity projects. Thanks to the amazing work of <a href="http://github.com/sschmid/Entitas-CSharp/">https://github.com/sschmid/Entitas-CSharp/</a>

</p>

Instructions
=============
* Replace the /Unity/Assets/3rdParty/Entitas folder contents with the latest download from <a href="http://github.com/sschmid/Entitas-CSharp/">github.com</a></BR>
* Open the /Unity/ folder in Unity3D. </BR>
* Open the EntitasTemplate.scene file. Play.

Structure Overview
=============
* **/Assets/RMC/Prototype/Common/Scripts/Runtime/** contains code that could be reused across various Entitas games<BR>
* **/Assets/RMC/Prototype/EntitasTemplate/Scripts/Runtime/** contains game-specific code

Code Overview
=============
* **GameController.cs** is the main entry point
* **GameConstants.cs** has some easy to edit values
* **StartNextRoundSystem.cs** is called before every round.

TODO
=============
* make this work ENTITAS_DISABLE_VISUAL_DEBUGGING
* use 'private' explicitely everywhere
* make all systems reactive that should be




Created BY
=============

- Samuel Asher Rivello <a href="https://twitter.com/srivello/">@srivello</a>, <a href="https://github.com/RivelloMultimediaConsulting">Github</a>, <a href="www.rivellomultimediaconsulting.com/unity/">Rivellomultimediaconsulting.com</a>

