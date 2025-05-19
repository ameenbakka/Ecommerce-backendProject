
﻿using AutoMapper;
using Ecommerce.AppDbContext;
using Ecommerce.DTO;
using Ecommerce.Models;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Services.AddressService
{
	public class AddressServices: IAddressService
	{
		private readonly AppdbContext _context;
		private readonly IMapper _mapper;
		public AddressServices(AppdbContext context,IMapper mapper)
		{
			_context = context;
			_mapper = mapper;
		}
		public async Task<ApiResponses<string>> AddAdress(int userid, createaddressdto addaddress)
		{
			if(addaddress == null)
			{
				return new ApiResponses<string>(400,"Address could not be null");
			}
			var addresscount =await _context.Addresses.CountAsync(x=>x.UserId == userid);
			if(addresscount >= 3)
			{
				return new ApiResponses<string>(400, "User cannot have more than 3 addresses.");
			}
			var address=_mapper.Map<Address>(addaddress);
			address.UserId= userid;
			await _context.Addresses.AddAsync(address);
			await _context.SaveChangesAsync();
			return new ApiResponses<string>(200, "Successfully Created Shipping Address");
		}
		public async Task<ApiResponses<List<ShowAddressDto>>> ShowAddresses(int userid)
		{
			var useradress=await _context.users.Include(x=>x.Addresses).FirstOrDefaultAsync(p=>p.Id==userid);
			if(useradress == null)
			{
				return new ApiResponses<List<ShowAddressDto>>(401, "invalid user");
			}
			var addresses=new List<ShowAddressDto>();
			foreach(var address in useradress.Addresses)
			{
				addresses.Add(_mapper.Map<ShowAddressDto>(address));
			}
			return new ApiResponses<List<ShowAddressDto>>(200,"Address is fetched successfully", addresses);
		}

		public async Task<bool> DeleteAddress(int userid, int addressid)
		{
			try 
			{
				var user =await _context.users.Include(c => c.Addresses).FirstOrDefaultAsync(x=>x.Id==userid);
				var checkaddress = user.Addresses.FirstOrDefault(x => x.AddressId == addressid);
				if(checkaddress == null || user == null)
				{
					return false;
				}
				user.Addresses.Remove(checkaddress);
				await _context.SaveChangesAsync();
				return true;
			}
			catch(Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

	}
}
