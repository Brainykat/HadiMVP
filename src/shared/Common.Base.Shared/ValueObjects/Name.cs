namespace Common.Base.Shared.ValueObjects
{
  public class Name
  {
		public static Name Create(string sur, string first, string? middle = default)
		{
			return new Name(sur, first, middle);
		}
		private Name(string sur, string first, string? middle = default)
		{
			if (string.IsNullOrWhiteSpace(sur)) throw new ArgumentNullException(nameof(FullName));
			Sur = sur.Trim();
			First = first != null ? first.Trim() : " ";
			Middle = middle != null ? middle.Trim() : null;
		}
		public string Sur { get; private set; }
		public string First { get; private set; }
		public string? Middle { get; private set; }
		public string FullName => $"{Sur} {First} {Middle}";
		public string FullNameReverse => $"{Middle} {Sur} {First}";
		public override string ToString() => $"{Sur} {First} {Middle}";
	}
}
