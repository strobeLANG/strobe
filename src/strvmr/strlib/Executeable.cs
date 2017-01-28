namespace StrobeVM
{
    /// <summary>
    /// Executeable.
    /// </summary>
    public class Executeable
	{
		/// <summary>
		/// Gets the type.
		/// </summary>
		/// <value>The type.</value>
		public Type type {get; private set;}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:StrobeVM.Executeable"/> class.
		/// </summary>
		/// <param name="type">Type.</param>
		public Executeable(Type type)
		{
			this.type = type;
		}

		/// <summary>
		/// Type.
		/// </summary>
		public enum Type
		{
			DIF, // Direct Instruction Format
		};

		/// <summary>
		/// Get the code ready for the CPU.
		/// </summary>
		public virtual Instruction[] CPU()
		{
			return new Instruction[] { };
		}
	}
}
