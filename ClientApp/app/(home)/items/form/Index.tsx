'use client'

import axiosService from "@/services/axiosService"
import FileUpload from "@/components/common/FileUpload/FileUploadOne"
import { useGlobalState } from "@/context/globalState"
import { Category } from "@/types/models/category"
import { Item } from "@/types/models/item"
import { useEffect, useState } from "react"
import { MultiSelect, Option } from "react-multi-select-component"
import { useRouter } from "next/navigation"

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
  const { user } = useGlobalState();
  const router = useRouter();

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
      startDate: new Date().toISOString().split('T')[0], // Set initial start date to today
      endDate: new Date().toISOString().split('T')[0],   // Set initial end date to today
    },
    categories: [],
  };

  const [formData, setFormData] = useState<SellItemPayload>(initialFormData);
  const [selectedCategories, setSelectedCategories] = useState<Option[]>([]);
  const [resMessage, setResMessage] = useState({
    title: "",
    description: "",
    categories: "",
    content: "",
    color: ""
  });

  useEffect(() => {
    if (item) {
      setFormData((prevFormData) => ({
        ...prevFormData,
        item: {
          ...prevFormData.item,
          ...item,
          startingPrice: prevFormData.item.startingPrice !== 0 ? prevFormData.item.startingPrice : item.startingPrice,
          increasingAmount: prevFormData.item.increasingAmount !== 0 ? prevFormData.item.increasingAmount : item.increasingAmount,
        },
      }));
    } if (existedCategories) {
      setSelectedCategories(existedCategories.map(cate => ({
        value: cate.categoryId,
        label: cate.categoryName,
        key: cate.description
      })))
    }
  }, [])

  const handleFileChange = (file: File) => {
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
          formDataToSend.append(`item.${key}`, value.toString());
        }
      });

      // Append imageFile to FormData
      if (formData.item.imageFile) {
        formDataToSend.append('item.ImageFile', formData.item.imageFile);
      }

      selectedCategories.forEach((category, index) => {
        formDataToSend.append(`categories[${index}].categoryId`, category.value.toString());
        formDataToSend.append(`categories[${index}].categoryName`, category.label);
        formDataToSend.append(`categories[${index}].Description`, category?.key ?? "");
      });

      await axiosService.post(`/api/user/${item == undefined ? "sellItem" : "updateItem"}`, formDataToSend, {
        headers: {
          'Content-Type': 'multipart/form-data',
        },
      }).then((res) => {
        if (res.status == 200) {
          setResMessage({
            ...resMessage,
            content: res.data?.message,
            color: "meta-4"
          })

          router.push("/items/" + res.data?.itemId);
        }
      }).catch((e) => {
        if (e?.response?.status === 400) {
          const errors = e?.response?.data?.errors;

          setResMessage({
            content: e?.response?.data?.message,
            title: errors?.['Item.Title'] ? errors['Item.Title'][0] : '',
            description: errors?.['Item.Description'] ? errors['Item.Description'][0] : '',
            categories: errors?.Categories ? errors.Categories[0] : '',
            color: "meta-1",
          });
        }


      })
    } catch (error) {
      console.error('Error during form submission', error);
    }
  };



  return (
    <form onSubmit={handleSubmit}>
      <p className={`text-${resMessage.color} text-center`}>{resMessage.content}</p>
      <div>
        <label className="mb-3 block text-black dark:text-white">Title</label>
        <p className={`text-${resMessage.color}`}>{resMessage.title}</p>

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
        <p className={`text-${resMessage.color}`}>{resMessage.description}</p>
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
        <p className="text-center text-meta-3">File choosen: {formData.item.imageFile?.name ?? "No file choosen"}</p>
        <div className="flex flex-row items-center">
          {item?.image && (
            <img src={item.image} className="w-36 h-36" alt="" />
          )}
        <FileUpload onFileChange={handleFileChange} />
        </div>
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
        <p className={`text-meta-1`}>{resMessage.categories}</p>

        <MultiSelect
          options={categories.map(cate => {
            return {
              label: cate.categoryName,
              value: cate.categoryId,
              key: cate.description
            }
          })}
          value={selectedCategories}
          onChange={setSelectedCategories}
          labelledBy="Select"
        />
      </div>

      <div>
        <label className="mb-3 block text-black dark:text-white">Start Date</label>
        <input
          type="date"
          value={formData.item.startDate.substring(0, 10)} // Extracting the date part
          onChange={(e) => setFormData((prev) => ({ ...prev, item: { ...prev.item, startDate: e.target.value }, }))}
          className="w-full rounded-lg border-[1.5px] border-primary bg-transparent py-3 px-5 font-medium outline-none transition focus:border-primary active:border-primary disabled:cursor-default disabled:bg-whiter dark:bg-form-input"
        />
      </div>

      <div>
        <label className="mb-3 block text-black dark:text-white">End Date</label>
        <input
          type="date"
          value={formData.item.endDate.substring(0, 10)} // Extracting the date part
          onChange={(e) => setFormData((prev) => ({ ...prev, item: { ...prev.item, endDate: e.target.value }, }))}
          className="w-full rounded-lg border-[1.5px] border-primary bg-transparent py-3 px-5 font-medium outline-none transition focus:border-primary active:border-primary disabled:cursor-default disabled:bg-whiter dark:bg-form-input"
        />
      </div>


      {/* Add more fields as needed */}

      <button type="submit" className="mt-4 bg-primary text-white py-3 px-6 rounded-lg">
        {!item ? "Create Item" : "Update Item"}
      </button>
    </form>
  );
}

export default Index
