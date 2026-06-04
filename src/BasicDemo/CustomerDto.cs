namespace BasicDemo
{
    public class CustomerDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public Guid Id { get; init; }

        /// <summary>
        /// 客户名称
        /// </summary>
        public string? Name { get; init; }

        /// <summary>
        /// 客户邮箱
        /// </summary>
        public string? Email{ get; init; }
    }
}
