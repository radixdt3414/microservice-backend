using discount.API.Data;
using discount.API.Protos;
using Grpc.Core;
using Mapster;
using Microsoft.EntityFrameworkCore;
using CouponModel = discount.API.Model.Coupon; 

namespace discount.API.Services
{
    public class DiscountService(DiscountContext db, ILogger<DiscountService> logger) : DiscountProtoService.DiscountProtoServiceBase
    {
        public override async Task<CouponResponse> AddDiscount(AddDiscountRequest request, ServerCallContext context)
        {
            var obj = request.Adapt<CouponModel>();

            if (obj == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"Discount with {request.ProductName} not found."));
            }
            await db.coupons.AddAsync(obj);
            await db.SaveChangesAsync();
            return obj.Adapt<CouponResponse>();
        }

        public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
        {
            var obj = await db.coupons.Where(x => x.ProductName == request.ProductName).FirstOrDefaultAsync();
            if(obj == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"Discount with {request.ProductName} not found."));
            }
            db.coupons.Remove(obj);
            await db.SaveChangesAsync();
            return new DeleteDiscountResponse()
            {
                IsSuccess = true
            };
        }

        public override async Task<CouponResponse> GetDiscount(GetDiscountRequest request, ServerCallContext context)
        {
            var obj = await db.coupons.Where(x => x.ProductName == request.ProductName).FirstOrDefaultAsync();
            var result = new CouponResponse();  
            if (obj == null)
            {
                result = new CouponResponse()
                {
                    Id = string.Empty,
                    ProductName = "No Discount",
                    DiscountPrice = 0,
                    Description = string.Empty
                };
            }
            else
            {
                result = obj.Adapt<CouponResponse>();
            }
            return result;
        }

        public override async Task<CouponResponse> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
        {
            try
            {
                var obj = request.Adapt<CouponModel>();
                if (obj == null)
                {
                    throw new RpcException(new Status(StatusCode.NotFound, $"Discount with {request.ProductName} not found."));
                }
                db.Attach<CouponModel>(obj);
                db.coupons.Update(obj);
                await db.SaveChangesAsync();
                return obj.Adapt<CouponResponse>();

            }
            catch (Exception ex)
            {
                throw new RpcException(new Status(StatusCode.Unknown, $"While update discount record encounter error."));
            }
        }
    }
}