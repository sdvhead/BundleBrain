<frame layout="850px 500px"
       
background={@Mods/StardewUI/Sprites/MenuBackground}
       
border={@Mods/StardewUI/Sprites/MenuBorder}
       
border-thickness="36, 36, 40, 36">
    
<lane layout="stretch content" orientation="vertical">
        
<label font="dialogue" text="Hello from StardewUI!" />
        
<label text={IntroParagraph} />
        
<textinput layout="200px content" text={<>FarmerName} />
        
<image layout="400px 100px" sprite={@Mods/TestMod/Sprites/Hello} />
        
<button text="Launch" click=|LaunchCoolFeature("now")| />
    
</lane>
</frame>