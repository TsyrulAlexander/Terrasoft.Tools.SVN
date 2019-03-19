using System.ComponentModel;
using Terrasoft.Tools.SvnUI.Model.Converter;

namespace Terrasoft.Tools.SvnUI.Model
{
	[TypeConverter(typeof(EnumDescriptionConverter))]
	public enum SvnOperation {
		NaN = 0,
		[Description("Создать фичу")]
		CreateFeature,
		[Description("Обновить фичу")]
		UpdateFeature,
		[Description("Завершить разработку")]
		FinishFeature,
		[Description("Закрыть ветку")]
		CloseFeature,
		[Description("Исправить ветку")]
		FixFeature
	}
}
