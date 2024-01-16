"use client"

import { useGlobalState } from '@/context/globalState';
import useColorMode from '@/hooks/useColorMode';
import { Bid } from '@/types/models/bid';
import { Category } from '@/types/models/category';
import { Item } from '@/types/models/item';
import Link from 'next/link';
import { usePathname, useRouter, useSearchParams } from 'next/navigation';
import React from 'react';
import ItemCard from './ItemCard';

const CategoryCard = ({ category }: { category: Category }) => {
  const {colorMode} = useGlobalState();

  return (
    <div className={`shadow-md rounded-lg p-4
      ${colorMode === 'dark' ? "bg-gray" : "bg-graydark"}
    `}>
      <h5 className="text-xl font-bold mb-2">{category.categoryName}</h5>
      <p className="text-gray-600 mb-4">{category.description}</p>
      <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4 2xl:grid-cols-5 gap-4">
        {category.categoryItems?.map((item) => (
          <ItemCard key={item.itemId} item={item.item} />
        ))}
      </div>
      {/* Linking Card */}
      <Link 
        href={`/items?category=${category.categoryId}`} 
        className={`block text-center mt-4 p-2 rounded hover:bg-blue-600 transition-colors
         ${colorMode === 'dark' ? 'bg-primary' : 'bg-secondary'}
          `}>
          View All Items
      </Link>
    </div>
  );
};


export default CategoryCard;