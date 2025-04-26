namespace E_CommerceLivraria.Extra
{
    public class NamespaceRoutingConvention : IOutboundParameterTransformer
    {
        public string TransformOutbound(object value)
        {
            if (value == null) return null;
            return value.ToString().Replace('.', '/');
        }
    }
}
