import { Bid } from "@/types/models/bid";
import { parseDate } from "@/utils";

const BidCard = ({ bid }: {bid: Bid}) => {
  console.log(bid);
  
  
    return (
      <div className="bg-white shadow-md rounded-lg p-4">
        <h5 className="text-xl font-bold mb-2">${bid.bidAmount}</h5>
        <p className="text-gray-600 mb-4">{parseDate(bid.bidDate).toDateString()}</p>
        {bid.user && (
          <div className="flex items-center mb-4">
            <img src={bid.user?.avatar} alt={bid.user.name} className="w-10 h-10 mr-2" />
            <p className="text-gray-600 mb-2">{bid.user.name}</p>
          </div>
        )}
      </div>
    );
  };

  export default BidCard;