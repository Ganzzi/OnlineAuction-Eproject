"use client"

import { SearchParams } from '@/types/next';
import { Resource, newResource } from '@/types/resource';
import { usePathname, useRouter, useSearchParams } from 'next/navigation';
import React, { useState } from 'react';
import { category1, category2, item1, item2 } from "@/data/item";
import { Category } from "@/types/models/category";
import { Item } from '@/types/models/item';
import Pagination from '@/components/common/Pagination/Pagination';
import ItemCard from '@/components/Home/ItemCard';

const categories: Category[] = [
  category1,
  category2
];

const Index = ({ searchParams, resource }: { searchParams: SearchParams, resource: Resource<Item> }) => {
  const router = useRouter();
  const pathname = usePathname();

  const [searchKeyword, setSearchKeyword] = useState<string>(searchParams?.search as string || '');
  const [sortBy, setSortBy] = useState<string>(searchParams?.sortBy as string || 'name');
  const [selectedCategory, setSelectedCategory] = useState(searchParams?.category as string || 'all');

  const handleSearch = () => {
    const newSearchParams = new URLSearchParams(searchParams);
    newSearchParams.set('search', searchKeyword);
    newSearchParams.set('sortBy', sortBy);
    newSearchParams.set('category', selectedCategory);

    router.push(`${pathname}?${newSearchParams}`);
  };

  const handleSortChange = (value: string) => {
    setSortBy(value);
    const newSearchParams = new URLSearchParams(searchParams);
    newSearchParams.set('sortBy', value);

    router.push(`${pathname}?${newSearchParams}`);
  };

  const handleCategoryChange = (value: string) => {
    setSelectedCategory(value);
    const newSearchParams = new URLSearchParams(searchParams);
    newSearchParams.set('category', value);

    router.push(`${pathname}?${newSearchParams}`);
  };

  return (
    <div className='flex flex-col items-end'>
      <button
        onClick={() => router.push('/items/form')}
        className="mt-2 px-4 py-2 bg-meta-5 hover:bg-meta-3 text-white rounded hover:bg-blue-600"
      >Sell Item</button>

      <div className="flex">
        {/* Left Section: Filter Bar */}
        <FilterBar
          searchKeyword={searchKeyword}
          sortBy={sortBy}
          selectedCategory={selectedCategory}
          onSearchChange={setSearchKeyword}
          onSortChange={handleSortChange}
          onCategoryChange={handleCategoryChange}
          onSearch={handleSearch}
          categories={categories} // Assuming you have a Category type
        />

        {/* Right Section: List of Item Cards and Pagination */}
        <div className="w-3/4 p-4 space-y-5">
          <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4 2xl:grid-cols-5 gap-4">
            {resource.data.map((item) => (
              <ItemCard key={item.itemId} item={item} />
            ))}
          </div>

          {/* Pagination Component */}
          <Pagination meta={resource.meta} />
        </div>
      </div>
    </div>
  );
};

interface FilterBarProps {
  searchKeyword: string;
  sortBy: string;
  selectedCategory: string;
  onSearchChange: (value: string) => void;
  onSortChange: (value: string) => void;
  onCategoryChange: (value: string) => void;
  onSearch: () => void;
  categories: Category[]; // Assuming you have a Category type
}

const FilterBar: React.FC<FilterBarProps> = ({
  searchKeyword,
  sortBy,
  selectedCategory,
  onSearchChange,
  onSortChange,
  onCategoryChange,
  onSearch,
  categories,
}) => {
  return (
    <div className="w-1/4 p-4">
      {/* Search Input */}
      <input
        type="text"
        placeholder="Search..."
        value={searchKeyword}
        onChange={(e) => onSearchChange(e.target.value)}
        className="w-full rounded border border-gray-300 p-2 mb-2"
      />

      {/* Search Button */}
      <button
        onClick={onSearch}
        className="w-full bg-primary text-white p-2 rounded hover:bg-secondary transition duration-300"
      >
        Search
      </button>

      {/* Sort Options */}
      <div className="mt-4">
        <label htmlFor="sort" className="block text-sm font-medium text-gray-700">
          Sort:
        </label>
        <select
          id="sort"
          value={sortBy}
          onChange={(e) => onSortChange(e.target.value)}
          className="w-full rounded border border-gray-300 p-2"
        >
          <option value="default">Default Sort</option>
          <option value="name">Sort by Name</option>
          {/* Add more sorting options as needed */}
        </select>
      </div>

      {/* Category Options */}
      <div className="mt-4">
        <label htmlFor="category" className="block text-sm font-medium text-gray-700">
          Category:
        </label>
        <select
          id="category"
          value={selectedCategory}
          onChange={(e) => onCategoryChange(e.target.value)}
          className="w-full rounded border border-gray-300 p-2"
        >
          <option value={999}>All Categories</option>
          {categories.map((category) => (
            <option key={category.categoryId} value={category.categoryId}>
              {category.categoryName}
            </option>
          ))}
        </select>
      </div>
    </div>

  );
};

export default Index;

