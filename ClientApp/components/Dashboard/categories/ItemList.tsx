import { CategoryData } from '@/app/(admin)/dashboard/categories/[categoryId]/page';
import axiosService from '@/services/axiosService';
import React, { useState } from 'react'

interface ItemListProps {
  items: CategoryData[];
  categoryId: number;
}

const ItemList: React.FC<ItemListProps> = ({ items, categoryId }) => {
  const [belongMap, setBelongMap] = useState<Record<number, boolean>>({});

  const handleButtonClick = async (itemId: number, belong: boolean) => {
    try {
      await axiosService.post(`/api/admin/AddDeleteCategoryItem/${categoryId}/${itemId}`);

      // Update the belong state in the local map
      setBelongMap((prevBelongMap) => ({ ...prevBelongMap, [itemId]: !belong }));
    } catch (error) {
      console.error('Error updating category item:', error);
    }
  };

  return (
    <div className="flex flex-col  p-6.5 items-center w-full">
      <div className="">

      {items.map((categoryData) => (
          <ItemComponent key={categoryData.item.itemId} categoryData={categoryData} categoryId={categoryId} />
        ))}
      </div>
    </div>
  );
};

const ItemComponent = ({ categoryData, categoryId }: { categoryData: CategoryData; categoryId: number }) => {
  const [isBelonged, setIsBelonged] = useState(categoryData.belong);

  const handleButtonClick = async () => {
    try {
      await axiosService.post(`/api/admin/AddDeleteCategoryItem/${categoryId}/${categoryData.item.itemId}`)
        .then(() => {
          setIsBelonged(!isBelonged);
        });
          
    } catch (error) {
      console.error('Error updating category item:', error);
    }
  };

  return (
    <div key={categoryData.item.itemId} className="item-info flex flex-row items-center justify-between space-x-10">
      <div className='flex flex-row items-center'>
        <img src={categoryData.item.image} className='w-20 h-20' alt="" />
        <div>
          <h3 className="text-lg font-semibold">{categoryData.item.title}</h3>
          <p className="text-gray-500">{categoryData.item.description}</p>
        </div>
        {/* Add more details as needed */}
      </div>

      <button
        onClick={handleButtonClick}
        className={`rounded-md bg-meta-3 py-2 px-5 text-center font-medium text-white hover:bg-opacity-90 ${
          isBelonged ? 'bg-meta-7' : 'bg-meta-3'
        } lg:px-8 xl:px-5`}
      >
        {isBelonged ? 'Remove' : 'Add'}
      </button>
    </div>
  );
};

export default ItemList;