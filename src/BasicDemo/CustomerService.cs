namespace BasicDemo
{
    public class CustomerService
    {
        private readonly ILogger<CustomerService> _logger;
        public CustomerService(ILogger<CustomerService> logger)
        {
            _logger = logger;
        }

        public List<CustomerDto> All()
        {
            var customers = new List<CustomerDto>
            {
                new() { Id = Guid.NewGuid(), Name = "张伟", Email = "zhangwei@example.com" },
                new() { Id = Guid.NewGuid(), Name = "李娜", Email = "lina@example.com" },
                new() { Id = Guid.NewGuid(), Name = "王芳", Email = "wangfang@example.com" },
                new() { Id = Guid.NewGuid(), Name = "刘洋", Email = "liuyang@example.com" },
                new() { Id = Guid.NewGuid(), Name = "陈杰", Email = "chenjie@example.com" },
                new() { Id = Guid.NewGuid(), Name = "杨敏", Email = "yangmin@example.com" },
                new() { Id = Guid.NewGuid(), Name = "赵磊", Email = "zhaolei@example.com" },
                new() { Id = Guid.NewGuid(), Name = "黄丽", Email = "huangli@example.com" },
                new() { Id = Guid.NewGuid(), Name = "周强", Email = "zhouqiang@example.com" },
                new() { Id = Guid.NewGuid(), Name = "吴婷", Email = "wuting@example.com" },
            };
            return customers;
        }

        public void Add()
        {
            _logger.LogInformation("新增客户信息");
        }

        public void Update()
        {
            _logger.LogInformation("更新客户信息");
        }

        public void Delete()
        {
            _logger.LogInformation("删除客户信息");
        }
    }
}
