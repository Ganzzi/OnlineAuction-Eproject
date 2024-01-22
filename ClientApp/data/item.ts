import { AuctionHistory } from "@/types/models/auctionHistory";
import { Bid } from "@/types/models/bid";
import { Category } from "@/types/models/category";
import { CategoryItem } from "@/types/models/categoryItem";
import { Item } from "@/types/models/item";
import { User } from "@/types/models/user";

// Fake data for User type
const user1: User = {
    userId: 1,
    name: 'john_doe',
    email: 'john.doe@example.com',
    password: 'password123',
    role: 'user',
    avatar : "/images/user/user-06.png"
};

const user2: User = {
    userId: 2,
    name: 'jane_smith',
    email: 'jane.smith@example.com',
    password: 'pass456',
    role: 'admin',avatar : "/images/user/user-06.png"
};


// Fake data for Item type
const item1: Item = {
    itemId: 101,
    title: 'Laptop',
    description: 'Powerful laptop for all your needs',
    image: 'https://imgs.search.brave.com/QYNbIFQtgZ-SvVfVypQ1mehSR4Vy_VCCSBV6nAeUgFQ/rs:fit:860:0:0/g:ce/aHR0cHM6Ly9tZWRp/YS5nZXR0eWltYWdl/cy5jb20vaWQvNDcx/MjA3MjYzL3Bob3Rv/L2J1c2luZXNzLW5l/d3NwYXBlci5qcGc_/cz02MTJ4NjEyJnc9/MCZrPTIwJmM9bVFa/RExPWmtKVmQ4MDVz/amdVREVmdV9ib09x/Q2d4NEFIZ1VoMVBM/Y1NYND0',
    startingPrice: 1200,
    increasingAmount: 20,
    reservePrice: 1500,
    startDate: new Date('2024-02-01T10:00:00Z').toDateString(),
    endDate: new Date('2024-02-10T18:00:00Z').toDateString(),
    sellerId: user1.userId,
    seller: user1,
  };
  
  const item2: Item = {
    itemId: 102,
    title: 'Smartphone',
    description: 'The latest smartphone with amazing features',
    image: 'https://imgs.search.brave.com/QYNbIFQtgZ-SvVfVypQ1mehSR4Vy_VCCSBV6nAeUgFQ/rs:fit:860:0:0/g:ce/aHR0cHM6Ly9tZWRp/YS5nZXR0eWltYWdl/cy5jb20vaWQvNDcx/MjA3MjYzL3Bob3Rv/L2J1c2luZXNzLW5l/d3NwYXBlci5qcGc_/cz02MTJ4NjEyJnc9/MCZrPTIwJmM9bVFa/RExPWmtKVmQ4MDVz/amdVREVmdV9ib09x/Q2d4NEFIZ1VoMVBM/Y1NYND0',
    startingPrice: 800,
    increasingAmount: 20,
    reservePrice: 1000,
    startDate: new Date('2024-02-15T12:00:00Z').toDateString(),
    endDate: new Date('2024-02-25T20:00:00Z').toDateString(),
    sellerId: user2.userId,
    seller: user2,
  };


const auctionHistory1: AuctionHistory = {
    autionHistoryId: 201,
    winningBid: 1300,
    endDate: new Date('2024-03-01T12:00:00Z'),
    winnerId: user1.userId,
    winner: user1,
    itemId: item1.itemId,
    item: item1,
};
  
const auctionHistory2: AuctionHistory = {
    autionHistoryId: 202,
    winningBid: 900,
    endDate: new Date('2024-03-02T14:30:00Z'),
    winnerId: user2.userId,
    winner: user2,
    itemId: item2.itemId,
    item: item2,
  };

// Fake data for Bid type
const bid1: Bid = {
    bidId: 201,
    bidAmount: 1300,
    bidDate: new Date('2024-01-15T08:00:00').toDateString(),
    userId: user2.userId,
    user: user2,
    itemId: item1.itemId,
    item: item1,
};

const bid2: Bid = {
    bidId: 202,
    bidAmount: 850,
    bidDate: new Date('2024-01-16T10:30:00').toDateString(),
    userId: user1.userId,
    user: user1,
    itemId: item2.itemId,
    item: item2,
};

// Fake data for Category type
const category1: Category = {
    categoryId: 1,
    categoryName: 'Category 1',
    description: 'Electronic gadgets and devices',
};

const category2: Category = {
    categoryId: 2,
    categoryName: 'Category 2',
    description: 'Fashionable clothing items',
};

// Fake data for CategoryItem type
const categoryItem1: CategoryItem = {
    categoryId: category1.categoryId,
    category: category1,
    itemId: item1.itemId,
    item: item1,
};

const categoryItem2: CategoryItem = {
    categoryId: category2.categoryId,
    category: category2,
    itemId: item2.itemId,
    item: item2,
};

  
export { category1, category2, item1, item2, user1, user2, categoryItem1, categoryItem2, bid1, bid2, auctionHistory1, auctionHistory2 };