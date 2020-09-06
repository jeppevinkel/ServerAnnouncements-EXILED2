using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.ObjectGraphVisitors;

namespace ServerAnnouncements.Api.YamlComments
{
	public class CommentsObjectGraphVisitor : ChainedObjectGraphVisitor
	{
		public CommentsObjectGraphVisitor(IObjectGraphVisitor<IEmitter> nextVisitor)
			: base(nextVisitor)
		{
		}

		public override bool EnterMapping(IPropertyDescriptor key, IObjectDescriptor value, IEmitter context)
		{
			var commentsDescriptor = value as CommentsObjectDescriptor;
			if (commentsDescriptor != null && commentsDescriptor.Comment != null)
			{
				context.Emit(new Comment(commentsDescriptor.Comment, false));
			}

			return base.EnterMapping(key, value, context);
		}
	}
}
