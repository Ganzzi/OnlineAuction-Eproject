import { Bid } from "@/types/models/bid";
import { Category } from "@/types/models/category";
import { CategoryItem } from "@/types/models/categoryItem";
import { Item } from "@/types/models/item";
import { User } from "@/types/models/user";

// Fake data for User type
const user1: User = {
    userId: 1,
    username: 'john_doe',
    email: 'john.doe@example.com',
    password: 'password123',
    role: 'user',
};

const user2: User = {
    userId: 2,
    username: 'jane_smith',
    email: 'jane.smith@example.com',
    password: 'pass456',
    role: 'admin',
};

// Fake data for Item type
const item1: Item = {
    itemId: 101,
    title: 'Laptop',
    description: 'Powerful laptop for all your needs',
    imgUrl: 'https://imgs.search.brave.com/QYNbIFQtgZ-SvVfVypQ1mehSR4Vy_VCCSBV6nAeUgFQ/rs:fit:860:0:0/g:ce/aHR0cHM6Ly9tZWRp/YS5nZXR0eWltYWdl/cy5jb20vaWQvNDcx/MjA3MjYzL3Bob3Rv/L2J1c2luZXNzLW5l/d3NwYXBlci5qcGc_/cz02MTJ4NjEyJnc9/MCZrPTIwJmM9bVFa/RExPWmtKVmQ4MDVz/amdVREVmdV9ib09x/Q2d4NEFIZ1VoMVBM/Y1NYND0',
    price: 1200,
    sellerId: user1.userId,
    seller: user1,
};

const item2: Item = {
    itemId: 102,
    title: 'Smartphone',
    description: 'The latest smartphone with amazing features',
    imgUrl: 'https://imgs.search.brave.com/QYNbIFQtgZ-SvVfVypQ1mehSR4Vy_VCCSBV6nAeUgFQ/rs:fit:860:0:0/g:ce/aHR0cHM6Ly9tZWRp/YS5nZXR0eWltYWdl/cy5jb20vaWQvNDcx/MjA3MjYzL3Bob3Rv/L2J1c2luZXNzLW5l/d3NwYXBlci5qcGc_/cz02MTJ4NjEyJnc9/MCZrPTIwJmM9bVFa/RExPWmtKVmQ4MDVz/amdVREVmdV9ib09x/Q2d4NEFIZ1VoMVBM/Y1NYND0',
    price: 800,
    sellerId: user2.userId,
    seller: user2,
};

// Fake data for Bid type
const bid1: Bid = {
    bidId: 201,
    bidAmount: 1300,
    bidDate: new Date('2024-01-15T08:00:00'),
    userId: user2.userId,
    user: user2,
    itemId: item1.itemId,
    item: item1,
};

const bid2: Bid = {
    bidId: 202,
    bidAmount: 850,
    bidDate: new Date('2024-01-16T10:30:00'),
    userId: user1.userId,
    user: user1,
    itemId: item2.itemId,
    item: item2,
};

// Fake data for Category type
const category1: Category = {
    categoryId: 301,
    categoryName: 'Electronics',
    description: 'Electronic gadgets and devices',
};

const category2: Category = {
    categoryId: 302,
    categoryName: 'Clothing',
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

export { category1, category2, item1, item2, user1, user2, categoryItem1, categoryItem2, bid1, bid2 };