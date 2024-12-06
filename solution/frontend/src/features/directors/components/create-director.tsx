import { Plus, X } from 'lucide-react'

import { Button } from '@/components/ui/button'
import { useNotifications } from '@/components/ui/notifications'
import { Form, FormDrawer, Input, Select } from '@/components/ui/form'

import { createDirectorInputSchema, useCreateDirector } from '../api/create-director'
import { useState } from 'react'
import { Movie } from '@/types/api'
import { useAllMovies } from '@/features/movies/api/get-movies'

export const CreateDirector = () => {
  const { addNotification } = useNotifications()
  const moviesAllQuery = useAllMovies({})

  const mutation = useCreateDirector({
    mutationConfig: {
      onSuccess: () => {
        addNotification({
          type: 'success',
          title: 'Director Created'
        })
      }
    }
  })

  const [reactiveMovies, setReactiveMovies] = useState<Movie[]>([])
  const [selectedMovie, setSelectedMovie] = useState<string | null>(null)

  const addMovie = (uniqueId: string) => {
    if (uniqueId) {
      const movie = moviesAllQuery.data?.find((movie: Movie) => movie.uniqueId === uniqueId)
      const movieExists = reactiveMovies.find((movie) => movie.uniqueId === uniqueId)
      if (movie && !movieExists) setReactiveMovies((prevMovies) => [...prevMovies, movie])
    }
    setSelectedMovie(null)
  }

  const removeMovie = (uniqueId: string) => {
    setReactiveMovies((prevMovies) => prevMovies.filter((movie) => movie.uniqueId !== uniqueId))
  }

  return (
    <FormDrawer
      isDone={mutation.isSuccess}
      triggerButton={
        <Button size="sm" icon={<Plus className="size-4" />}>
          Create Director
        </Button>
      }
      title="Create Director"
      submitButton={
        <Button form="create-director" type="submit" size="sm">
          Submit
        </Button>
      }
    >
      <Form
        id="create-director"
        onSubmit={(values) => {
          values.movies = reactiveMovies
          mutation.mutate({ data: values })
        }}
        schema={createDirectorInputSchema}
      >
        {({ register, formState }) => (
          <>
            <Input label="Name" error={formState.errors['name']} registration={register('name')} />

            <div className="grid grid-cols-2 gap-4 content-end">
              <Select
                label="Movies"
                className="border border-gray-300 rounded-md"
                defaultValue={null}
                options={(moviesAllQuery?.data ?? []).map((movie: Movie) => ({
                  value: movie.uniqueId,
                  label: movie.title
                }))}
                registration={register('movies', {
                  onChange: (e) => setSelectedMovie(e.target.value)
                })}
              ></Select>
              <div className="content-end">
                <Button
                  type="button"
                  icon={<Plus className="size-4" />}
                  onClick={() => selectedMovie && addMovie(selectedMovie)}
                >
                  Add
                </Button>
              </div>
            </div>

            <ul>
              {reactiveMovies.map((movie, index) => (
                <li key={index} className="ml-5 text-gray-700 list-none flex ">
                  {movie.title}
                  <Button
                    type="button"
                    onClick={() => removeMovie(movie.uniqueId)}
                    className="content-center size-1 m-1"
                  >
                    <X className="size-4" />
                  </Button>
                </li>
              ))}
            </ul>
          </>
        )}
      </Form>
    </FormDrawer>
  )
}
