namespace Datateam.Foundation
{
	public interface IOrgService
	{
		Task<Tenant> CreatOrgDataBase(Guid tenantId);
	}
}
