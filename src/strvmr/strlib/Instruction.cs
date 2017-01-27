namespace StrobeVM
{
	/// <summary>
	/// Instruction.
	/// </summary>
	public class Instruction
	{
		/// <summary>
		/// Gets the operation type.
		/// </summary>
		/// <value>The operation type.</value>
		public OpType Op { get; private set; }

		/// <summary>
		/// Gets the parameters.
		/// </summary>
		/// <value>The parameters.</value>
		public byte[] Param { get; private set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="T:StrobeVM.Instruction"/> class.
		/// </summary>
		/// <param name="Op">Operation Type.</param>
		/// <param name="Param">Parameters.</param>
		public Instruction(OpType Op, byte[] Param)
		{
			this.Op = Op;
			this.Param = Param;
		}

		/// <summary>
		/// Operator Type.
		/// </summary>
		public enum OpType
		{
			Null, // Nothing.
			Add, // Add two numbers
			Subtract, // Subtract two numbers
			Divide, // Divide two numbers
			Mutiply, // Multiply two numbers
			Allocate, // Allocate space
			Assign, // Asign data to variable
			Interrupt, // Interrupt
			Compare, // Compare
			Move, // Move data from variable to variable
			Addr, // Move address from variable to variable
			Goto, // Jump to a label
			Label, // Define a label
			Clear, // Clear a variable
		}
	}
}
