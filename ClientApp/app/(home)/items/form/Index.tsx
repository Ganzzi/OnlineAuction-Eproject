'use client'

import axiosService from "@/axiosService"
import FileUpload from "@/components/common/FileUpload/FileUploadOne"
import { useGlobalState } from "@/context/globalState"
import { Category } from "@/types/models/category"
import { Item } from "@/types/models/item"
import { useEffect, useState } from "react"
import { MultiSelect, Option } from "react-multi-select-component"

type PageProps = {
  item?: Item,
  existedCategories?: Category[],
  categories: Category[]
}

type SellItemPayload = {
  item: Item,
  categories: Category[]
}

const Index: React.FC<PageProps> = ({ item, categories, existedCategories }) => {
  console.log(existedCategories);
  
  const {user} = useGlobalState();
  const initialFormData: SellItemPayload = {
    item: {
      itemId: 0,
      title: '',
      description: '',
      image: 'null',
      imageFile: undefined,
      startingPrice: 0,
      increasingAmount: 0,
      reservePrice: undefined,
      // sellerId: user.userId,
      sellerId: undefined,
      seller: undefined,
      categoryItems: undefined,
      bids: undefined,
      startDate: new Date().toDateString(),
      endDate: new Date().toDateString()
    },
    categories: [],
  };

  const [formData, setFormData] = useState<SellItemPayload>(initialFormData);
  const [selectedCategories, setSelectedCategories] = useState<Option[]>([]);

  useEffect(() => {
    if (item) {
      setFormData((prevFormData) => ({
        ...prevFormData,
        item: item,
      }));
    } if (existedCategories) {
      setSelectedCategories(existedCategories.map(cate => ({
        value: cate.categoryId,
        label: cate.categoryName
      })))
    }
  }, [])

  const handleFileChange = (file: File) => {
    console.log('get a file: '+ file.name);
    
    setFormData((prevFormData) => ({
      ...prevFormData,
      item: {
        ...prevFormData.item,
        imageFile: file,
      },
    }));
  };

  const handleSubmit = async (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault();
  
    try {
      const formDataToSend = new FormData();
  
      // Append item fields to FormData
      Object.entries(formData.item).forEach(([key, value]) => {
        // Check for undefined values before appending to FormData
        if (value !== undefined && value !== null) {
          console.log("appended");
          formDataToSend.append(`item.${key}`, value.toString());
        }
      });
  
      // Append imageFile to FormData
      if (formData.item.imageFile) {
        console.log("appended");
        formDataToSend.append('item.ImageFile', formData.item.imageFile);
      }

      selectedCategories.forEach((category, index) => {
        formDataToSend.append(`categories[${index}].categoryId`, category.value.toString());
        formDataToSend.append(`categories[${index}].categoryName`, category.label);
      });

      console.log(Array.from(formDataToSend));
  
      const res = await axiosService.post(`/api/user/${item == undefined ? "sellItem" : "updateItem"}`, formDataToSend, {
        headers: {
          'Content-Type': 'multipart/form-data',
        },
      });
  
      console.log(res);
  
    } catch (error) {
      console.error('Error during form submission', error);
    }
  };
  
  

  return (
    <form onSubmit={handleSubmit}>
      <div>
        <label className="mb-3 block text-black dark:text-white">Title</label>
        <input
          type="text"
          placeholder="Title"
          value={formData.item.title}
          onChange={(e) => setFormData((pev) => ({ ...pev, item: { ...pev.item, title: e.target.value }, }))}
          className="w-full rounded-lg border-[1.5px] border-primary bg-transparent py-3 px-5 font-medium outline-none transition focus:border-primary active:border-primary disabled:cursor-default disabled:bg-whiter dark:bg-form-input"
        />
      </div>

      <div>
        <label className="mb-3 block text-black dark:text-white">Description</label>
        <input
          type="text"
          placeholder="Description"
          value={formData.item.description}
          onChange={(e) => setFormData((pev) => ({ ...pev, item: { ...pev.item, description: e.target.value }, }))}
          className="w-full rounded-lg border-[1.5px] border-primary bg-transparent py-3 px-5 font-medium outline-none transition focus:border-primary active:border-primary disabled:cursor-default disabled:bg-whiter dark:bg-form-input"
        />
      </div>
      <div>
        <label className="mb-3 block text-black dark:text-white">File</label>
        <FileUpload onFileChange={handleFileChange} />
      </div>

      <div>
        <label className="mb-3 block text-black dark:text-white">Starting Price</label>
        <input
          type="number"
          placeholder="Starting Price"
          value={formData.item.startingPrice}
          onChange={(e) => setFormData((pev) => ({ ...pev, item: { ...pev.item, startingPrice: Number(e.target.value) }, }))}
          className="w-full rounded-lg border-[1.5px] border-primary bg-transparent py-3 px-5 font-medium outline-none transition focus:border-primary active:border-primary disabled:cursor-default disabled:bg-whiter dark:bg-form-input"
        />
      </div>

      <div>
        <label className="mb-3 block text-black dark:text-white">Increasing Amount</label>
        <input
          type="number"
          placeholder="Increasing Amount"
          value={formData.item.increasingAmount}
          onChange={(e) => setFormData((pev) => ({ ...pev, item: { ...pev.item, increasingAmount: Number(e.target.value) }, }))}
          className="w-full rounded-lg border-[1.5px] border-primary bg-transparent py-3 px-5 font-medium outline-none transition focus:border-primary active:border-primary disabled:cursor-default disabled:bg-whiter dark:bg-form-input"
        />
      </div>

      <div>
        <label className="mb-3 block text-black dark:text-white">Reserve Price</label>
        <input
          type="number"
          placeholder="Reserve Price"
          value={formData.item.reservePrice || ''}
          onChange={(e) => setFormData((pev) => ({ ...pev, item: { ...pev.item, reservePrice: Number(e.target.value) }, }))}
          className="w-full rounded-lg border-[1.5px] border-primary bg-transparent py-3 px-5 font-medium outline-none transition focus:border-primary active:border-primary disabled:cursor-default disabled:bg-whiter dark:bg-form-input"
        />
      </div>

      <div>
        <label className="mb-3 block text-black dark:text-white">Categories</label>
        <MultiSelect
          options={categories.map(cate => {
            return {
              label: cate.categoryName,
              value: cate.categoryId,
            }
          })}
          value={selectedCategories}
          onChange={setSelectedCategories}
          labelledBy="Select"
        />
      </div>

      {/* Add more fields as needed */}

      <button type="submit" className="mt-4 bg-primary text-white py-3 px-6 rounded-lg">
        Create Item
      </button>
    </form>
  );
}

export default Index
