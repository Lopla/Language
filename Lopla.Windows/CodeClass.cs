namespace Lopla.Windows
{
    public class CodeClass
    {
        public string DrawLines;

        public CodeClass()
        {
            this.Perf = @"
lastCall = 0
function Perf.Start(){
    lastCall=Lp.Ticks()
}

function Perf.Stop(){
    ticksElapsed = Lp.Ticks()

    duration =ticksElapsed - lastCall 
    seconds =(duration) / 10000000 
    Draw.Log(seconds)
}

";


            DrawLines =
                Perf +

                @"

function Draw.Test(){
    Perf.Start()

    Draw.Clear(0,0,0)
    Draw.SetColor(0,255,0)
    line = 0
    while(line < 1000){
    col = 0
    while(col < 100){
        Draw.Line(line,col, line,col+1)
        col = col +1
    }
    line = line + 1
    Draw.Flush()
    }

    Draw.SetColor(255,255,255)
    Perf.Stop()
    Draw.Flush()
}

while (1)
{
    Draw.WaitForEvent()
    Draw.Test()
}

";
        }

        public string Perf { get; set; }
    }
}