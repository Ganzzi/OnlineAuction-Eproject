import { CategoryData } from '@/app/(admin)/dashboard/items/[itemId]/page';
import axiosService from '@/services/axiosService';
import React, { useState } from 'react'

const CategoryList = ({categories, itemId} :{categories: CategoryData[], itemId: number}) => {
  return (
    <div className="bg-body p-4 rounded-md">
          <h3 className="text-xl font-bold mb-4">Categories</h3>
          {categories.map((categoryData) => (
            <CategoryComponent
              key={categoryData.category.categoryId}
              categoryData={categoryData}
              itemId={itemId}
            />
          ))}
        </div>
  )
}


const CategoryComponent = ({ categoryData, itemId }: { categoryData: CategoryData; itemId: number }) => {
  const { category, belong } = categoryData;
  const [isBelonged, setIsBelonged] = useState(belong);

  const handleButtonClick = async () => {
    try {
      await axiosService.post(`/api/admin/AddDeleteCategoryItem/${category.categoryId}/${itemId}`);
      setIsBelonged(!isBelonged);
    } catch (error) {
      console.error('Error handling button click:', error);
    }
  };

  return (
    <div key={category.categoryId} className="flex flex-row items-center justify-between bg-black p-2 m-2 rounded-md">
      <div>
      <p className="font-semibold">{category.categoryName}</p>
      <p className="text-gray-500">{category.description}</p>
      </div>
      <button
        onClick={handleButtonClick}
        className={`mt-2 px-4 py-2 rounded-md ${
          isBelonged ? 'bg-meta-7 text-white' : 'bg-meta-5 text-white'
        } hover:bg-opacity-80 focus:outline-none`}
      >
        {isBelonged ? 'Remove' : 'Add'}
      </button>
    </div>
  );
};

export default CategoryList