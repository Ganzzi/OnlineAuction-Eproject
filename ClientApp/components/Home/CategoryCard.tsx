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
  const router = useRouter();

  return (
    <div className={`shadow-md rounded-lg p-4
      ${colorMode === 'dark' ? "bg-bodydark1" : "bg-graydark"}
    `}>
      <h5 className="text-xl font-bold mb-2 text-meta-6">{category.categoryName}</h5>
      <p className="text-meta-8 mb-4">{category.description}</p>
      <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4 2xl:grid-cols-5 gap-4">
        {category.categoryItems?.map((item, i) => (
          <ItemCard key={i} item={item.item} />
        ))}
      </div>
      {/* Linking Card */}
      <Link 
        href={`/items?cate=${category.categoryId}`} 
        className={`block text-center mt-6 p-2 rounded hover:bg-meta-3 transition-colors
         ${colorMode === 'dark' ? 'bg-primary' : 'bg-secondary'}
          `}>
          View All Items
      </Link>
    </div>
  );
};


export default CategoryCard;