namespace StrobeVM
{
	/// <summary>
	/// Format.
	/// </summary>
	public abstract class Format
	{
		/// <summary>
		/// Load the specified Input.
		/// </summary>
		/// <param name="Input">Input.</param>
		public abstract Executeable Load(byte[] Input);
	}
}
