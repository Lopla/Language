namespace Lopla.Language.Compiler.Mnemonics
{
    using Binary;
    using Hime.Redist;
    using Processing;
    
    public class Nop : Mnemonic
    {
        public Nop(ASTNode? node) : base(node)
        {
        }


        public override Result Execute(Runtime runtime)
        {
            return new Result();
        }
    }
}