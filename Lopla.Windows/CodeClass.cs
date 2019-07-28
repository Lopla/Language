namespace Lopla.Windows
{
    public class CodeClass
    {
        public static string Perf = @"
lastCall = 0
function Perf.Start(){
    lastCall=Lp.Ticks()
}

function Perf.Stop(){
    ticksElapsed = Lp.Ticks()

    duration =ticksElapsed - lastCall 
    seconds = (duration) /   10000000 
    Perf.Show()
}

function Perf.Show(){
    Draw.SetColor(255,255,255)

    ticksElapsed = Lp.Ticks()
    duration = ticksElapsed - lastCall 
    seconds = (duration) /   10000000 
    
    
    Draw.SetColor(0,0,0)
    Draw.Box(0,0, 100, 30)
    Draw.SetColor(255,255,255)
    Draw.Text(10,10, seconds)
}
";


        public static string Anim = @"

function Anim.Test(){
    Perf.Start()

    /*Draw.Clear(0,0,0)*/
    line = 0
    while(line < 1000){
        col = 0
        while(col < 100){
        
            Draw.SetColor(0,255,0)
            Draw.Line(line,col, line,col+1)
            col = col +1
            Perf.Show()
        }
        Draw.Flush()
        line = line + 1
    }

    Draw.SetColor(255,255,255)

    Perf.Stop()
    Draw.Flush()
}

Draw.Clear(1,1,1)
Draw.Flush()
img = IO.LoadBinaryFile(""c:\\work\\kon.gif"")

while (1)
{
    Anim.Test()

    Draw.WaitForEvent()
    Draw.Animation(20,20,img)
    Draw.Flush()
}

";
    }
}